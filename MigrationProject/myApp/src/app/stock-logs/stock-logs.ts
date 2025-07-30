import { Component, inject, ViewEncapsulation } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Menu } from '../menu/menu';

@Component({
  selector: 'app-stock-logs',
  imports: [FormsModule, CommonModule],
  templateUrl: './stock-logs.html',
  styleUrl: './stock-logs.css',
  encapsulation: ViewEncapsulation.None 

})
export class StockLogs {
  private productservice = inject(ProductService);
  stocklogs: any[] = [];
  filteredlogs: any[] = [];
  searchByName: string = '';
  searchBywhoupdated: string = '';
  searchByWhenMin: string = '';
  searchByWhenMax: string = '';
  sortAsc: boolean = true;
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

downloadCSV() {
  const headers = ['Inventory ID', 'Product Name', 'Old Stock', 'New Stock', 'Updated At', 'Updated By', 'Status'];
  const rows = this.filteredlogs.map(log => [
    log.inventoryId,
    log.prodName,
    log.oldStock,
    log.newStock,
    log.updatedAt,
    log.updatedBy,
    log.newStock > log.oldStock ? 'Add' : (log.newStock < log.oldStock ? 'Reduce' : 'No Change')
  ]);

  const csvContent = [headers, ...rows].map(e => e.join(",")).join("\n");
  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement("a");
  const url = URL.createObjectURL(blob);
  link.setAttribute("href", url);
  link.setAttribute("download", "logs.csv");
  link.style.visibility = 'hidden';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

  ngOnInit(): void {
    this.handlestocklogs();
  }

  handlestocklogs() {
    this.productservice.getallstocklogs().subscribe({
      next: (data: any) => {
        this.stocklogs = data;

        // Load product names
        data.forEach((log: any) => {
          this.productservice.getprodbyinv(log.inventoryId).subscribe({
            next: (productData: any) => {
              log.prodName = productData[0]?.name ?? '';
            }
          });
        });
        this.filteredlogs = this.stocklogs;
      }
    });
  }

filterLogs() {
  this.filteredlogs = this.stocklogs.filter((log: any) => {
    const matchName = log.prodName?.toLowerCase().includes(this.searchByName.toLowerCase());
    const matchUpdatedBy = log.updatedBy?.toLowerCase().includes(this.searchBywhoupdated.toLowerCase());

    const matchDateMin = this.searchByWhenMin
      ? this.formatLocalDate(new Date(log.updatedAt)) >= this.searchByWhenMin
      : true;

    const matchDateMax = this.searchByWhenMax
      ? this.formatLocalDate(new Date(log.updatedAt)) <= this.searchByWhenMax
      : true;
    return matchName && matchUpdatedBy && matchDateMin && matchDateMax;
  });
}

// ✅ Helper method
formatLocalDate(date: Date): string {
  // Returns YYYY-MM-DD in local time
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

  trackByInventoryId(index: number, item: any): number {
    return item.inventoryId;
  }
}
