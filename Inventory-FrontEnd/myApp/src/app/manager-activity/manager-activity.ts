import { Component, inject, OnInit } from '@angular/core';
import { AdminManagerService } from '../services/AdminManager.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manager-activity',
  imports: [CommonModule, FormsModule],
  templateUrl: './manager-activity.html',
  styleUrl: './manager-activity.css'
})
export class ManagerActivity implements OnInit{
  private admmanservice = inject(AdminManagerService);
  allmanagers: any;
  selectedmanager:string='';
  report:any;
  ngOnInit(): void {
    this.handleallmanagers();
  }
  handleallmanagers()
  {
    this.admmanservice.getallmanagers().subscribe({
      next:(data:any)=>{
        this.allmanagers=data;
        console.log(this.allmanagers);
      }
    })
  }
  handlemanageractivity()
  {
    this.admmanservice.handlemanagerreport(this.selectedmanager).subscribe({
      next:(data:any)=>{
        this.report=data;
        console.log(data);
      }
    })
  }
  trackByName(index: number, item: any): string {
  return item.name;
}
}
