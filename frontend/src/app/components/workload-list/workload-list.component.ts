import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Workload } from '../../models/entities';

@Component({
  selector: 'app-workload-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './workload-list.component.html',
  styleUrl: './workload-list.component.css'
})
export class WorkloadListComponent implements OnInit {
  workloads: Workload[] = [];
  loading = false;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadWorkloads();
  }

  loadWorkloads() {
    this.loading = true;
    this.apiService.getWorkloads().subscribe({
      next: (data) => {
        this.workloads = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading workloads:', err);
        this.loading = false;
      }
    });
  }

  deleteWorkload(id: string) {
    if (confirm('Are you sure you want to delete this workload?')) {
      this.apiService.deleteWorkload(id).subscribe({
        next: () => this.loadWorkloads(),
        error: (err) => console.error('Error deleting workload:', err)
      });
    }
  }
}

