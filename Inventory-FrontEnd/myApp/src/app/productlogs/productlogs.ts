import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Menu } from '../menu/menu';

@Component({
  selector: 'app-productlogs',
  imports: [CommonModule, FormsModule,],
  templateUrl: './productlogs.html',
  styleUrl: './productlogs.css',
  encapsulation: ViewEncapsulation.None 

})
export class Productlogs implements OnInit{
  private prodservice = inject(ProductService);

  prodlogs:any;
  filteredlogs:any;
  searchByName:string="";
  searchBywhoupdated:string="";
  searchByWhenMin: string = '';
  searchByWhenMax: string = '';
  sortAsc: boolean = true;
  searchByFieldUpdated:string="";
  currentPage = 1;
itemsPerPage = 14;

get pages() {
  return Array(Math.ceil(this.filteredlogs.length / this.itemsPerPage)).fill(0);
}

get paginatedLogs() {
  const start = (this.currentPage - 1) * this.itemsPerPage;
  return this.filteredlogs.slice(start, start + this.itemsPerPage);
}

goToPage(page: number) {
  this.currentPage = page;
}

prevPage() {
  if (this.currentPage > 1) this.currentPage--;
}

nextPage() {
  if (this.currentPage < this.pages.length) this.currentPage++;
}

downloadCSV() {
  const headers = ['Product Name', 'Field Updated', 'Old Value', 'New Value', 'Updated At', 'Updated By'];
  const rows = this.filteredlogs.map((log:any) => [
    log.prodName,
    log.fieldUpdated,
    log.oldValue,
    log.newValue,
    log.updatedAt,
    log.updatedBy
  ]);
  const csvContent = [headers, ...rows].map(row => row.join(",")).join("\n");
  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement("a");
  const url = URL.createObjectURL(blob);
  link.setAttribute("href", url);
  link.setAttribute("download", "product_logs.csv");
  link.style.visibility = 'hidden';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

  ngOnInit(): void {
    this.loadproductupdatelogs();
  }
  loadproductupdatelogs()
  {
    this.prodservice.getallproductlogs().subscribe({
      next:(data:any)=>{
        this.prodlogs = data;
        // console.log(this.prodlogs);

        data.forEach((p:any) => {
          // console.log(p.id);
          this.prodservice.getprodnamebyid(p.productId).subscribe({
            next:(data:any)=>{
              // console.log(data);
              p.prodName = data.name;
              console.log(p.prodName);
            }
          })
        });
        this.filteredlogs = this.prodlogs;

      }
    })
  }
  filterLogs() {
  this.filteredlogs = this.prodlogs.filter((log: any) => {
    const matchName = log.prodName?.toLowerCase().includes(this.searchByName.toLowerCase());
    const matchUpdatedBy = log.updatedBy?.toLowerCase().includes(this.searchBywhoupdated.toLowerCase());
    const matchbyfield = this.searchByFieldUpdated?log.fieldUpdated==this.searchByFieldUpdated.toUpperCase():true;
    const matchDateMin = this.searchByWhenMin
      ? this.formatLocalDate(new Date(log.updatedAt)) >= this.searchByWhenMin
      : true;

    const matchDateMax = this.searchByWhenMax
      ? this.formatLocalDate(new Date(log.updatedAt)) <= this.searchByWhenMax
      : true;
    return matchName && matchUpdatedBy && matchDateMin && matchDateMax && matchbyfield;
  });
}
  formatLocalDate(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

  sortByDate() {
    this.filteredlogs = this.filteredlogs.sort((a: any, b: any) =>
      this.sortAsc
        ? new Date(a.updatedAt).getTime() - new Date(b.updatedAt).getTime()
        : new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime()
    );
    this.sortAsc = !this.sortAsc;
  }
  trackById(index: number, item: any): number {
    return item.id;
  }
}
