import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Workload, Runtime, SoftwareComponent, ApiDefinition, MessageConnection, Database } from '../../models/entities';
import { Cardinality } from '../../models/enums';

@Component({
  selector: 'app-workload-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './workload-form.component.html',
  styleUrl: './workload-form.component.css'
})
export class WorkloadFormComponent implements OnInit {
  workload: Partial<Workload> = {
    name: '',
    repoUrl: '',
    tag: '',
    softwareComponentIds: [],
    apisExposedIds: [],
    apisInvokedIds: [],
    consumeMessageFromIds: [],
    produceMessageToIds: [],
    databaseIds: []
  };
  isEdit = false;
  loading = false;

  runtimes: Runtime[] = [];
  softwareComponents: SoftwareComponent[] = [];
  apis: ApiDefinition[] = [];
  messageConnections: MessageConnection[] = [];
  databases: Database[] = [];

  constructor(
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.loadWorkload(id);
    }
    this.loadDependencies();
  }

  loadWorkload(id: string) {
    this.loading = true;
    this.apiService.getWorkload(id).subscribe({
      next: (data) => {
        this.workload = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading workload:', err);
        this.loading = false;
      }
    });
  }

  loadDependencies() {
    this.apiService.getRuntimes().subscribe(data => this.runtimes = data);
    this.apiService.getSoftwareComponents().subscribe(data => this.softwareComponents = data);
    this.apiService.getApis().subscribe(data => this.apis = data);
    this.apiService.getMessageConnections().subscribe(data => this.messageConnections = data);
    this.apiService.getDatabases().subscribe(data => this.databases = data);
  }

  toggleSelection(array: string[], id: string) {
    const index = array.indexOf(id);
    if (index > -1) {
      array.splice(index, 1);
    } else {
      array.push(id);
    }
  }

  isSelected(array: string[], id: string): boolean {
    return array.includes(id);
  }

  save() {
    this.loading = true;
    const workloadData = {
      ...this.workload,
      id: this.isEdit ? this.workload.id : undefined
    } as Workload;

    const operation = this.isEdit
      ? this.apiService.updateWorkload(workloadData.id!, workloadData)
      : this.apiService.createWorkload(workloadData);

    operation.subscribe({
      next: () => {
        this.router.navigate(['/workloads']);
      },
      error: (err) => {
        console.error('Error saving workload:', err);
        this.loading = false;
      }
    });
  }
}

