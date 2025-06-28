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
  paginationState: { [key: string]: number } = {};
  
  filterQueries: { [key: string]: string } = {};

getFilteredData(key: string, data: any[]): any[] {
  const query = this.filterQueries[key]?.toLowerCase() || '';
  if (!query) return data;
  return data.filter(item =>
    Object.values(item).some(val =>
      String(val).toLowerCase().includes(query)
    )
  );
}

getFilteredAndPaginatedData(key: string, data: any[], pageSize = 5): any[] {
  const filtered = this.getFilteredData(key, data);
  const page = this.getPage(key);
  const start = (page - 1) * pageSize;
  return filtered.slice(start, start + pageSize);
}

getTotalPages(filteredData: any[], pageSize = 5): number {
  return Math.ceil(filteredData.length / pageSize);
}



  getPage(key: string): number {
    return this.paginationState[key] || 1;
  }

  setPage(key: string, value: number): void {
    this.paginationState[key] = value;
  }

  getPaginatedData(key: string, items: any[], itemsPerPage: number = 5): any[] {
    const page = this.getPage(key);
    const start = (page - 1) * itemsPerPage;
    return items.slice(start, start + itemsPerPage);
  }

  
//   pageSize = 5;

//   pagination:any = {
//     products: 0,
//     categories: 0,
//     managers: 0,
//     inventory: 0,
//     productUpdates: 0
//   };

//   searchQueries:any = {
//     products: '',
//     categories: '',
//     managers: '',
//     inventory: '',
//     productUpdates: ''
//   };
  userid:string='';
  role:string='';
  report:any='';

//   productsSection: any = null;
//   categoriesSection: any = null;
//   managersSection: any = null;
//   inventorySection: any = null;
//   productUpdatesSection: any = null;
  private admmanservice = inject(AdminManagerService);
  ngOnInit(): void {
  const storedUserId = localStorage.getItem("userid");
  if (storedUserId) {
    this.userid = storedUserId;
    if (this.userid.includes('ADM')) {
      this.role = 'ADMIN';
    } else {
      this.role = 'MANAGER';
    }

    // ✅ Moved here so role is available
    this.handlereport();
  }
}
  sections: { title: string, items: string[] }[] = [];

handlereport() {
  if (this.role === 'ADMIN') {
    this.admmanservice.handleadminreport(this.userid).subscribe({
      next: (data: any) => {
        this.report = data;
        console.log(data);

      },
      error: (err) => {
       
      },
    });
  }
  else
  {
    this.admmanservice.handlemanagerreport(this.userid).subscribe({
      next: (data: any) => {
        this.report = data;
        console.log(data);
        console.log(Object.keys(data))
      },
      error: (err) => {

      },
    })
  }
}
// parseReport(data: string) {
//   this.sections = [];
//   const lines = data.split('\n');
//   let currentSection: { title: string, items: string[] } | null = null;

//   lines.forEach(line => {
//     line = line.trim();
//     if (!line || /^-+$/.test(line)) {
//       return;
//     }
//     if (line.startsWith('•')) {
//       currentSection?.items.push(line.replace('•', '').trim());
//     } else {
//       if (currentSection) {
//         this.sections.push(currentSection);
//       }
//       currentSection = {
//         title: line.replace(':', '').trim(),
//         items: []
//       };
//     }
//   });
//   if (currentSection) {
//     this.sections.push(currentSection);
//   }

//   // Now categorize sections
//   this.productsSection = this.sections.find(s =>
//     s.title.toLowerCase().includes('product') && s.title.toLowerCase().includes('created')
//   );
//   this.categoriesSection = this.sections.find(s =>
//     s.title.toLowerCase().includes('category') || s.title.toLowerCase().includes('categories')
//   );
//   this.managersSection = this.sections.find(s =>
//     s.title.toLowerCase().includes('manager')
//   );
//   this.inventorySection = this.sections.find(s =>
//     s.title.toLowerCase().includes('inventory')
//   );
//   this.productUpdatesSection = this.sections.find(s =>
//     s.title.toLowerCase().includes('product') && s.title.toLowerCase().includes('update')
//   );
// }
// filterItems(data: any, section: string): any[] {
//     const query = this.searchQueries[section]?.toLowerCase();
//     if (!query) return data.items;

//     return data.items.filter((item: string) =>
//       item.toLowerCase().includes(query)
//     );
//   }
// nextPage(section: string, data: any) {
//     if ((this.pagination[section] + 1) * this.pageSize < data.items.length) {
//       this.pagination[section]++;
//     }
//   }

//   prevPage(section: string) {
//     if (this.pagination[section] > 0) {
//       this.pagination[section]--;
//     }
//   }

//   getPageData(data: any, section: string): any[] {
//     const filtered = this.filterItems(data, section);
//     const start = this.pagination[section] * this.pageSize;
//     return filtered.slice(start, start + this.pageSize);
//   }
  
}
