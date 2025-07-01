declare const html2pdf: any;

import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { AdminManagerService } from '../services/AdminManager.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manager-activity',
  imports: [CommonModule, FormsModule],
  templateUrl: './manager-activity.html',
  styleUrl: './manager-activity.css',
  encapsulation: ViewEncapsulation.None
})
export class ManagerActivity implements OnInit {
  private admmanservice = inject(AdminManagerService);
  allmanagers: any;
  selectedmanager: string = '';
  report: any;
  reportFilter:any;
  ngOnInit(): void {
    this.handleallmanagers();
  }
  setselectedmanager()
  {
    this.handlemanageractivity();
  }
  handleallmanagers() {
    this.admmanservice.getallmanagers().subscribe({
      next: (data: any) => {
        this.allmanagers = data;
        console.log(this.allmanagers);
      }
    });
  }

  handlemanageractivity() {
    this.admmanservice.handlemanagerreport(this.selectedmanager).subscribe({
      next: (data: any) => {
        this.report = data;
        console.log(data);
      }
    });
  }

  trackByName(index: number, item: any): string {
    return item.name;
  }

  downloadPDF() {
    const reportElement = document.getElementById('report-wrapper');
    if (!reportElement) return;

    const options = {
      margin: 0.5,
      filename: 'Manager_Report.pdf',
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
    };

    html2pdf().set(options).from(reportElement).save();
  }
  get filteredReport() {
  if (!this.reportFilter?.trim()) return this.report;

  const term = this.reportFilter.toLowerCase();
  const filterText = (val: any) => JSON.stringify(val).toLowerCase().includes(term);

  return {
    productAdded: this.report.productAdded?.filter(filterText) || [],
    stockUpds: this.report.stockUpds?.filter(filterText) || [],
    prodUpds: this.report.prodUpds?.filter(filterText) || [],
    allRequest: this.report.allRequest?.filter(filterText) || []
  };
}

}
