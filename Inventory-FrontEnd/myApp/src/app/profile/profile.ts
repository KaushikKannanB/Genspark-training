import { Component, inject, OnInit } from '@angular/core';
import { AdminManagerService } from '../services/AdminManager.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit{
  pageSize = 5;

  pagination:any = {
    products: 0,
    categories: 0,
    managers: 0,
    inventory: 0,
    productUpdates: 0
  };

  searchQueries:any = {
    products: '',
    categories: '',
    managers: '',
    inventory: '',
    productUpdates: ''
  };
  userid:string='';
  role:string='';
  report:string='';
  showChangePasswordModal = false;
  newPassword = '';
  productsSection: any = null;
  categoriesSection: any = null;
  managersSection: any = null;
  inventorySection: any = null;
  productUpdatesSection: any = null;
  private admmanservice = inject(AdminManagerService);
  ngOnInit(): void {
    const storedUserId = localStorage.getItem("userid");
    if (storedUserId) {
      this.userid = storedUserId;
      if(this.userid.includes('ADM'))
      {
        this.role='ADMIN';
      }
      else
      {
        this.role='MANAGER';
      }
    }
    this.handlereport();
  }
  sections: { title: string, items: string[] }[] = [];

handlereport() {
  if (this.role === 'ADMIN') {
    this.admmanservice.handleadminreport(this.userid).subscribe({
      next: (data: any) => {
        this.report = data;
        this.parseReport(data);
      },
      error: (err) => {
        this.report = err.error.text;
        console.log(this.report);
        this.parseReport(err.error.text);
      },
    });
  }
  else
  {
    this.admmanservice.handlemanagerreport(this.userid).subscribe({
      next: (data: any) => {
        this.report = data;
        this.parseReport(data);
      },
      error: (err) => {
        this.report = err.error.text;
        console.log(this.report);
        this.parseReport(err.error.text);
      },
    })
  }
}
parseReport(data: string) {
  this.sections = [];
  const lines = data.split('\n');
  let currentSection: { title: string, items: string[] } | null = null;

  lines.forEach(line => {
    line = line.trim();
    if (!line || /^-+$/.test(line)) {
      return;
    }
    if (line.startsWith('•')) {
      currentSection?.items.push(line.replace('•', '').trim());
    } else {
      if (currentSection) {
        this.sections.push(currentSection);
      }
      currentSection = {
        title: line.replace(':', '').trim(),
        items: []
      };
    }
  });
  if (currentSection) {
    this.sections.push(currentSection);
  }

  // Now categorize sections
  this.productsSection = this.sections.find(s =>
    s.title.toLowerCase().includes('product') && s.title.toLowerCase().includes('created')
  );
  this.categoriesSection = this.sections.find(s =>
    s.title.toLowerCase().includes('category') || s.title.toLowerCase().includes('categories')
  );
  this.managersSection = this.sections.find(s =>
    s.title.toLowerCase().includes('manager')
  );
  this.inventorySection = this.sections.find(s =>
    s.title.toLowerCase().includes('inventory')
  );
  this.productUpdatesSection = this.sections.find(s =>
    s.title.toLowerCase().includes('product') && s.title.toLowerCase().includes('update')
  );
}
filterItems(data: any, section: string): any[] {
    const query = this.searchQueries[section]?.toLowerCase();
    if (!query) return data.items;

    return data.items.filter((item: string) =>
      item.toLowerCase().includes(query)
    );
  }
nextPage(section: string, data: any) {
    if ((this.pagination[section] + 1) * this.pageSize < data.items.length) {
      this.pagination[section]++;
    }
  }

  prevPage(section: string) {
    if (this.pagination[section] > 0) {
      this.pagination[section]--;
    }
  }

  getPageData(data: any, section: string): any[] {
    const filtered = this.filterItems(data, section);
    const start = this.pagination[section] * this.pageSize;
    return filtered.slice(start, start + this.pageSize);
  }
  
  
}
