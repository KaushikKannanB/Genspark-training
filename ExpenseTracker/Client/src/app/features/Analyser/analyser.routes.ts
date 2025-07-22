import { Routes } from '@angular/router';
import { CategoryTableComponent } from '@features/Admin/category/components/category-table.component';
import { Analyser } from './analyser/analyser';

export const analyserRoutes: Routes = [
  {
    path: '',
    component: Analyser,
    title: 'analyser',
  }
];
