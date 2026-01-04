import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { WorkloadListComponent } from './components/workload-list/workload-list.component';
import { WorkloadFormComponent } from './components/workload-form/workload-form.component';
import { VisualizationComponent } from './components/visualization/visualization.component';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'workloads', component: WorkloadListComponent },
  { path: 'workloads/new', component: WorkloadFormComponent },
  { path: 'workloads/:id', component: WorkloadFormComponent },
  { path: 'workloads/:id/edit', component: WorkloadFormComponent },
  { path: 'visualization', component: VisualizationComponent },
  { path: '**', redirectTo: '' }
];
