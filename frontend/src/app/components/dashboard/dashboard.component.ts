import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  entities = [
    { name: 'Workloads', route: '/workloads', icon: '⚙️' },
    { name: 'APIs', route: '/apis', icon: '🔌' },
    { name: 'Connections', route: '/connections', icon: '🔗' },
    { name: 'Brokers', route: '/brokers', icon: '📡' },
    { name: 'Topics', route: '/topics', icon: '📬' },
    { name: 'Message Schemas', route: '/message-schemas', icon: '📋' },
    { name: 'Message Connections', route: '/message-connections', icon: '💬' },
    { name: 'Runtimes', route: '/runtimes', icon: '🚀' },
    { name: 'Databases', route: '/databases', icon: '💾' },
    { name: 'Software Components', route: '/software-components', icon: '🧩' },
    { name: 'Publishers', route: '/publishers', icon: '📦' },
    { name: 'Architecture Visualization', route: '/visualization', icon: '📊' }
  ];
}

