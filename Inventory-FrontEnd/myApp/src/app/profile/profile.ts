import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { AdminManagerService } from '../services/AdminManager.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
  encapsulation: ViewEncapsulation.None
})
export class Profile implements OnInit {
  userid: string = '';
  role: string = '';
  report: any = null;

  private admmanservice = inject(AdminManagerService);

  // Tracks current page per section
  paginationState: Record<string, number> = {};

  // Items per page per section
  pageSize: Record<string, number> = {
    prodsAdded: 5,
    stockUpds: 5,
    managers: 5,
    prodUpds: 5,
    categadds: 5,
    productAdded: 5,
    allRequest: 5
  };

  // Global filter used for all sections
  filterQueries: Record<string, string> = {
    global: ''
  };

  ngOnInit(): void {
    const storedUserId = localStorage.getItem("userid");
    if (storedUserId) {
      this.userid = storedUserId;
      this.role = this.userid.includes('ADM') ? 'ADMIN' : 'MANAGER';
      this.handlereport();
    }
  }

  handlereport(): void {
    if (this.role === 'ADMIN') {
      this.admmanservice.handleadminreport(this.userid).subscribe({
        next: (data: any) => {
          this.report = data;
        },
        error: (err) => {
          console.error('Admin report fetch error:', err);
        }
      });
    } else {
      this.admmanservice.handlemanagerreport(this.userid).subscribe({
        next: (data: any) => {
          this.report = data;
        },
        error: (err) => {
          console.error('Manager report fetch error:', err);
        }
      });
    }
  }

  // Pagination helpers
  getPage(key: string): number {
    return this.paginationState[key] || 1;
  }

  setPage(key: string, value: number): void {
    const totalPages = this.getTotalPages(this.getFilteredData(key, this.report?.[key] || []), this.pageSize[key]);
    this.paginationState[key] = Math.max(1, Math.min(value, totalPages));
  }

  getTotalPages(data: any[], size: number = 5): number {
    return Math.ceil(data.length / size);
  }

  // Filtering
  getFilteredData(key: string, data: any[]): any[] {
    const query = this.filterQueries['global'].toLowerCase() || '';
    if (!query) return data;
    return data.filter(item =>
      Object.values(item).some(val =>
        String(val).toLowerCase().includes(query)
      )
    );
  }

  getFilteredAndPaginatedData(key: string, data: any[]): any[] {
    const filtered = this.getFilteredData(key, data);
    const page = this.getPage(key);
    const size = this.pageSize[key] || 5;
    const start = (page - 1) * size;
    return filtered.slice(start, start + size);
  }

  // For single-line display in manager card layout
  renderLine(item: any, section?: string): string {
  if (section === 'productAdded') {
    return `${item.id} – ${item.name}`;
  }
  return Object.values(item).join(' – ');
}
}
