import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Menu } from '../menu/menu';

@Component({
  selector: 'app-productlogs',
  imports: [CommonModule, FormsModule,],
  templateUrl: './productlogs.html',
  styleUrl: './productlogs.css'
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
