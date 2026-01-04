import { Component, OnInit, OnDestroy, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import * as d3 from 'd3';
import {
  Workload, Broker, Topic, MessageConnection, ApiDefinition,
  Database, Runtime, Connection
} from '../../models/entities';

interface GraphNode {
  id: string;
  type: string;
  name: string;
  data?: any;
}

interface GraphLink {
  source: string | GraphNode;
  target: string | GraphNode;
  type: string;
  label?: string;
}

@Component({
  selector: 'app-visualization',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './visualization.component.html',
  styleUrl: './visualization.component.css'
})
export class VisualizationComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('svgContainer', { static: false }) svgContainer!: ElementRef<SVGElement>;
  
  private svg: any;
  private simulation: any;
  private nodes: GraphNode[] = [];
  private links: GraphLink[] = [];
  private nodeElements: any;
  private linkElements: any;
  private labelElements: any;
  
  loading = false;
  width = 1200;
  height = 800;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadData();
  }

  ngAfterViewInit() {
    this.initVisualization();
  }

  ngOnDestroy() {
    if (this.simulation) {
      this.simulation.stop();
    }
  }

  loadData() {
    this.loading = true;
    Promise.all([
      this.apiService.getWorkloads().toPromise(),
      this.apiService.getBrokers().toPromise(),
      this.apiService.getTopics().toPromise(),
      this.apiService.getMessageConnections().toPromise(),
      this.apiService.getApis().toPromise(),
      this.apiService.getDatabases().toPromise(),
      this.apiService.getRuntimes().toPromise(),
      this.apiService.getConnections().toPromise()
    ]).then(([workloads, brokers, topics, messageConnections, apis, databases, runtimes, connections]) => {
      this.buildGraph(
        workloads || [],
        brokers || [],
        topics || [],
        messageConnections || [],
        apis || [],
        databases || [],
        runtimes || [],
        connections || []
      );
      this.loading = false;
      setTimeout(() => this.updateVisualization(), 100);
    }).catch(err => {
      console.error('Error loading data:', err);
      this.loading = false;
    });
  }

  buildGraph(
    workloads: Workload[],
    brokers: Broker[],
    topics: Topic[],
    messageConnections: MessageConnection[],
    apis: ApiDefinition[],
    databases: Database[],
    runtimes: Runtime[],
    connections: Connection[]
  ) {
    this.nodes = [];
    this.links = [];

    // Add nodes
    workloads.forEach(w => {
      this.nodes.push({ id: `workload-${w.id}`, type: 'workload', name: w.name, data: w });
    });

    brokers.forEach(b => {
      this.nodes.push({ id: `broker-${b.id}`, type: 'broker', name: b.name, data: b });
    });

    topics.forEach(t => {
      this.nodes.push({ id: `topic-${t.id}`, type: 'topic', name: t.name, data: t });
    });

    apis.forEach(a => {
      this.nodes.push({ id: `api-${a.id}`, type: 'api', name: a.name, data: a });
    });

    databases.forEach(d => {
      this.nodes.push({ id: `database-${d.id}`, type: 'database', name: d.name, data: d });
    });

    runtimes.forEach(r => {
      this.nodes.push({ id: `runtime-${r.id}`, type: 'runtime', name: r.name, data: r });
    });

    // Add links
    // Topics to Brokers
    topics.forEach(topic => {
      if (topic.brokerId) {
        this.links.push({
          source: `topic-${topic.id}`,
          target: `broker-${topic.brokerId}`,
          type: 'serves',
          label: 'serves'
        });
      }
    });

    // Workloads to Topics (consume)
    workloads.forEach(workload => {
      workload.consumeMessageFrom?.forEach(mc => {
        if (mc.topic) {
          this.links.push({
            source: `topic-${mc.topic.id}`,
            target: `workload-${workload.id}`,
            type: 'consumed-by',
            label: 'consumes'
          });
        }
      });
    });

    // Workloads to Topics (produce)
    workloads.forEach(workload => {
      workload.produceMessageTo?.forEach(mc => {
        if (mc.topic) {
          this.links.push({
            source: `workload-${workload.id}`,
            target: `topic-${mc.topic.id}`,
            type: 'produces',
            label: 'produces'
          });
        }
      });
    });

    // Workloads to APIs (exposed)
    workloads.forEach(workload => {
      workload.apisExposed?.forEach(api => {
        this.links.push({
          source: `workload-${workload.id}`,
          target: `api-${api.id}`,
          type: 'exposes',
          label: 'exposes'
        });
      });
    });

    // Workloads to APIs (invoked)
    workloads.forEach(workload => {
      workload.apisInvoked?.forEach(api => {
        this.links.push({
          source: `workload-${workload.id}`,
          target: `api-${api.id}`,
          type: 'invokes',
          label: 'invokes'
        });
      });
    });

    // Workloads to Databases
    workloads.forEach(workload => {
      workload.databases?.forEach(db => {
        this.links.push({
          source: `workload-${workload.id}`,
          target: `database-${db.id}`,
          type: 'uses-db',
          label: 'uses'
        });
      });
    });

    // Workloads to Runtimes
    workloads.forEach(workload => {
      if (workload.runtime) {
        this.links.push({
          source: `workload-${workload.id}`,
          target: `runtime-${workload.runtime.id}`,
          type: 'runs-on',
          label: 'runs-on'
        });
      }
    });

    // Connections
    connections.forEach(conn => {
      this.links.push({
        source: conn.fromId,
        target: conn.toId,
        type: 'connection',
        label: conn.name
      });
    });
  }

  initVisualization() {
    if (!this.svgContainer) return;

    const container = d3.select(this.svgContainer.nativeElement);
    container.selectAll('*').remove();

    this.svg = container
      .append('svg')
      .attr('width', this.width)
      .attr('height', this.height);

    // Add zoom behavior
    const zoom = d3.zoom()
      .scaleExtent([0.1, 4])
      .on('zoom', (event) => {
        this.svg.select('g').attr('transform', event.transform);
      });

    this.svg.call(zoom);

    const g = this.svg.append('g');

    // Create arrow markers
    g.append('defs').selectAll('marker')
      .data(['end'])
      .enter().append('marker')
      .attr('id', 'arrowhead')
      .attr('viewBox', '0 -5 10 10')
      .attr('refX', 25)
      .attr('refY', 0)
      .attr('markerWidth', 6)
      .attr('markerHeight', 6)
      .attr('orient', 'auto')
      .append('path')
      .attr('d', 'M0,-5L10,0L0,5')
      .attr('fill', '#999');

    this.linkElements = g.append('g').attr('class', 'links');
    this.nodeElements = g.append('g').attr('class', 'nodes');
    this.labelElements = g.append('g').attr('class', 'labels');

    this.updateVisualization();
  }

  updateVisualization() {
    if (!this.svg || !this.nodeElements) return;

    // Convert links to d3 format with string IDs
    const d3Links = this.links.map(link => ({
      source: typeof link.source === 'string' ? link.source : (link.source as GraphNode).id,
      target: typeof link.target === 'string' ? link.target : (link.target as GraphNode).id,
      type: link.type,
      label: link.label
    }));

    // Create force simulation
    this.simulation = d3.forceSimulation(this.nodes as any)
      .force('link', d3.forceLink(d3Links).id((d: any) => d.id).distance(100))
      .force('charge', d3.forceManyBody().strength(-300))
      .force('center', d3.forceCenter(this.width / 2, this.height / 2))
      .force('collision', d3.forceCollide().radius(30));

    // Get the link force to access transformed links
    const linkForce = this.simulation.force('link') as any;

    // Update links
    const link = this.linkElements
      .selectAll('line')
      .data(linkForce.links(), (d: any) => `${d.source.id}-${d.target.id}-${d.type || ''}`);

    link.exit().remove();

    const linkEnter = link.enter()
      .append('line')
      .attr('stroke', '#999')
      .attr('stroke-opacity', 0.6)
      .attr('stroke-width', 2)
      .attr('marker-end', 'url(#arrowhead)');

    linkEnter.merge(link as any);

    // Update nodes
    const node = this.nodeElements
      .selectAll('circle')
      .data(this.nodes, (d: any) => d.id);

    node.exit().remove();

    const nodeEnter = node.enter()
      .append('circle')
      .attr('r', 15)
      .attr('fill', (d: any) => this.getNodeColor(d.type))
      .attr('stroke', '#fff')
      .attr('stroke-width', 2)
      .call(this.drag(this.simulation) as any);

    nodeEnter.merge(node as any);

    // Update labels
    const label = this.labelElements
      .selectAll('text')
      .data(this.nodes, (d: any) => d.id);

    label.exit().remove();

    const labelEnter = label.enter()
      .append('text')
      .text((d: any) => d.name)
      .attr('font-size', '12px')
      .attr('dx', 20)
      .attr('dy', 5)
      .attr('fill', '#333');

    labelEnter.merge(label as any);

    // Add tooltips
    nodeEnter.append('title').text((d: any) => `${d.type}: ${d.name}`);

    // Update positions on tick
    this.simulation.on('tick', () => {
      link
        .attr('x1', (d: any) => d.source.x)
        .attr('y1', (d: any) => d.source.y)
        .attr('x2', (d: any) => d.target.x)
        .attr('y2', (d: any) => d.target.y);

      node
        .attr('cx', (d: any) => d.x)
        .attr('cy', (d: any) => d.y);

      label
        .attr('x', (d: any) => d.x)
        .attr('y', (d: any) => d.y);
    });
  }

  getNodeColor(type: string): string {
    const colors: { [key: string]: string } = {
      workload: '#4CAF50',
      broker: '#2196F3',
      topic: '#FF9800',
      api: '#9C27B0',
      database: '#F44336',
      runtime: '#00BCD4'
    };
    return colors[type] || '#757575';
  }

  drag(simulation: any) {
    function dragstarted(event: any) {
      if (!event.active) simulation.alphaTarget(0.3).restart();
      event.subject.fx = event.subject.x;
      event.subject.fy = event.subject.y;
    }

    function dragged(event: any) {
      event.subject.fx = event.x;
      event.subject.fy = event.y;
    }

    function dragended(event: any) {
      if (!event.active) simulation.alphaTarget(0);
      event.subject.fx = null;
      event.subject.fy = null;
    }

    return d3.drag()
      .on('start', dragstarted)
      .on('drag', dragged)
      .on('end', dragended);
  }
}

