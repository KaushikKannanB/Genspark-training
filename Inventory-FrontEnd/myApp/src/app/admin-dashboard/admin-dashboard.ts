import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { AdminManagerService } from '../services/AdminManager.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserCreate } from '../models/UserCreateModel';
import { User } from '../models/User';
import { Router } from '@angular/router';
import { NotificationService } from '../services/Notification.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [FormsModule, CommonModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
  encapsulation: ViewEncapsulation.None
})
export class AdminDashboard implements OnInit {
  private admmanservice = inject(AdminManagerService);
  private notify = inject(NotificationService);
  private router = inject(Router);
  alladmins:any;
  allmanagers:any;
  categoryrequests:any;
  newcategory:string='';
  newuser:UserCreate=new UserCreate();
  adminPage = 1;
  managerPage = 1;
  itemsPerPage = 5;
  categoryPage = 1;



  adminSearch = '';
  managerSearch = '';
  categorySearch:string='';
  categoryStatus:string='';
  managersearchstatus:string='';

  get paginatedAdmins() {
    return this.alladmins
      ?.filter((a: any) =>
        a.name.toLowerCase().includes(this.adminSearch.toLowerCase()) 
      )
      .slice((this.adminPage - 1) * this.itemsPerPage, this.adminPage * this.itemsPerPage);
  }

  get paginatedManagers() {
  const statusOrder: Record<'ACTIVE' | 'INACTIVE', number> = {
    ACTIVE: 0,
    INACTIVE: 1
  };

  const filtered = this.allmanagers
    ?.filter((m: any) => {
      const matchesName = m.name.toLowerCase().includes(this.managerSearch.toLowerCase());
      const matchesStatus = this.managersearchstatus
        ? m.status === this.managersearchstatus
        : true;

      return matchesName && matchesStatus;
    })
    .sort((a: any, b: any) => {
      return statusOrder[a.status as keyof typeof statusOrder] - statusOrder[b.status as keyof typeof statusOrder];
    });

  return filtered.slice(
    (this.managerPage - 1) * this.itemsPerPage,
    this.managerPage * this.itemsPerPage
  );
}



  get totalAdminPages() {
    return Math.ceil(
      (this.alladmins?.filter((a: any) =>
        a.name.toLowerCase().includes(this.adminSearch.toLowerCase())
      )?.length || 0) / this.itemsPerPage
    );
  }

  get totalManagerPages() {
    return Math.ceil(
      (this.allmanagers?.filter((m: any) =>
        m.name.toLowerCase().includes(this.managerSearch.toLowerCase())
      )?.length || 0) / this.itemsPerPage
    );
  }


  prevAdminPage() {
    if (this.adminPage > 1) this.adminPage--;
  }
  nextAdminPage() {
    if (this.adminPage < this.totalAdminPages) this.adminPage++;
  }
  prevManagerPage() {
    if (this.managerPage > 1) this.managerPage--;
  }
  nextManagerPage() {
    if (this.managerPage < this.totalManagerPages) this.managerPage++;
  }
  get paginatedCategoryRequests() {
  const order: Record<'REQUESTED' | 'ADDED' | 'DENIED', number> = {
    REQUESTED: 0,
    ADDED: 1,
    DENIED: 2
  };

  const filtered = this.categoryrequests
    ?.filter((req: any) => {
      const matchesName = req.categoryName
        .toLowerCase()
        .includes(this.categorySearch.toLowerCase());

      const matchesStatus = this.categoryStatus
        ? req.status === this.categoryStatus
        : true;

      return matchesName && matchesStatus;
    })
    .sort((a: any, b: any) => {
      return order[a.status as keyof typeof order] - order[b.status as keyof typeof order];
    });

  return filtered.slice(
    (this.categoryPage - 1) * this.itemsPerPage,
    this.categoryPage * this.itemsPerPage
  );
}



  get totalCategoryPages() {
    return Math.ceil(
      (this.categoryrequests?.filter((req: any) => {
        const matchesName = req.categoryName
          .toLowerCase()
          .includes(this.categorySearch.toLowerCase());

        const matchesStatus = this.categoryStatus
          ? req.status === this.categoryStatus
          : true;

        return matchesName && matchesStatus;
      })?.length || 0) / this.itemsPerPage
    );
  }

  prevCategoryPage() {
    if (this.categoryPage > 1) this.categoryPage--;
  }
  nextCategoryPage() {
    if (this.categoryPage < this.totalCategoryPages) this.categoryPage++;
  }


  acceptRequest(req:string) {
    console.log(req);
    this.admmanservice.addCategory(req).subscribe({
      next:(data:any)=>{
        console.log(data);
      }
    })
    alert("Catgeory added successfully");
    this.router.navigate(["home"]);
  }
  denyRequest(req:string, by:string)
  {
    this.admmanservice.cancelcategoryaddrequest(req).subscribe({
      next:(data:any)=>{
        console.log(data);
      }
    })
    // this.notify.messages.push(`Category request by ${by} for ${req} DENIED`);
    alert("Request rejected successfully");
    this.router.navigate(["home"]);
  }
  addcategory() {
    // console.log(req);
    this.admmanservice.addCategory(this.newcategory).subscribe({
      next:(data:any)=>{
        console.log(data);
      }
    })
    alert("Catgeory added successfully");
    this.router.navigate(["home"]);
  }
  ngOnInit(): void {
    this.handlealladmins();
    this.handleallmanagers();
    this.handlecategoryrequest();
  }
  handlealladmins()
  {
    this.admmanservice.getalladmins().subscribe({
      next:(data:any)=>{
        this.alladmins=data;
      }
    })
  }
  handleallmanagers()
  {
    this.admmanservice.getallmanagers().subscribe({
      next:(data:any)=>{
        this.allmanagers=data;
        console.log(data)

      }
    })
  }
  handlecategoryrequest()
  {
    this.admmanservice.getallcategoryrequests().subscribe({
      next:(data:any)=>{
        this.categoryrequests = data;
        console.log(data);
      }
    })
  }
  handlecreateuser()
  {
    if(this.newuser.role=='Manager')
    {
      this.admmanservice.createManager(this.newuser).subscribe({
        next:(data:any)=>{
          console.log(data);
        }
      })
    }
    if(this.newuser.role=='Admin')
    {
      this.admmanservice.createAdmin(this.newuser).subscribe({
        next:(data:any)=>{
          console.log(data);
        }
      })
    }
    alert("User created Successfully.")
    this.router.navigate(["home"]);
  }
}
