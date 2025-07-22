import { Component, inject, OnInit, signal } from '@angular/core';
import { AnalyserService } from '../services/AnalyserService';
import { TopUserChartComponent } from '../top-user-chart.component';
import { TopCategoryChartComponent } from '../top-category-chart.component';
import { UserSpendingTrendsComponent } from '../user-spending-trends.component';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { CategoryApiService } from '@features/category/services/category-api.service';
import { NotificationService } from '@core/services/notification.service';
import { LastExpensesChartComponent } from '../last-5-expenses.component';
import { TopDatesChartComponent } from '../top-5-dates.component';
import { TopCategoryFreqChartComponent } from '../top-5-freq-categories.component';
import { TopCategoryAmountChartComponent } from '../top-5-spent-categories.component';

@Component({
  selector: 'app-analyser',
  standalone: true,
  imports: [TopUserChartComponent, TopCategoryChartComponent, UserSpendingTrendsComponent, LastExpensesChartComponent, TopDatesChartComponent, TopCategoryFreqChartComponent, TopCategoryAmountChartComponent, CommonModule, FormsModule],
  templateUrl: './analyser.html',
})
export class Analyser implements OnInit {
  private service = inject(AnalyserService);
  private categ_service = inject(CategoryApiService);
  private toast = inject(NotificationService);
  topUsers: any[] = [];
  topCategories: any[] = [];
  allCategories: any[] = [];
  allUsers: any[] = [];
  selectedUser = '';
  selectMail = '';
  userExpenses: any[] = [];
  suggestion:string = '';
  isAddModalOpen = signal(false);
  filters = {
    categoryId: '',
    minAmount: 0,
    maxAmount: 0,
    fromDate: '',
    toDate: ''
  };

  top5expensesuser:any[]=[];
  latest5expensesuser:any[]=[];
  top5categoriesuser:any[]=[];
  top5categoriesuserfreq:any[]=[];

  getcategories() {
    this.service.gettopcategories().subscribe({
      next: (data: any) => (this.topCategories = data),
    });
  }

  getusers() {
    this.service.gettopusers().subscribe({
      next: (data: any) => (this.topUsers = data),
    });
  }

  getallusers() {
    this.service.getallusers().subscribe({
      next: (data: any) => {this.allUsers = data; console.log(data);},
    });
  }

  getexpensesbyid() {
    if (!this.selectedUser) return;
    console.log(this.selectedUser);
    this.service.getexpensebyid(this.selectedUser).subscribe({
      next: (data: any) => {
        this.userExpenses = data;

        this.service.getexpenses_latest5_byid(this.selectedUser).subscribe({
          next:(data:any)=>{
            this.latest5expensesuser = data;
            console.log(this.latest5expensesuser);
          }
        });

        this.service.getexpenses_top5_byid(this.selectedUser).subscribe({
          next:(data:any)=>{
            this.top5expensesuser = data;
            console.log(this.top5expensesuser);
          }
        });

        this.service.gettop5categories_by_user(this.selectedUser).subscribe({
          next:(data:any)=>{
            this.top5categoriesuser = data;
            console.log(this.top5categoriesuser);
          }
        });

        this.service.gettop5categories_by_user_freq(this.selectedUser).subscribe({
          next:(data:any)=>{
            this.top5categoriesuserfreq = data;
            console.log(this.top5categoriesuserfreq);
          }
        });


        console.log('User Expenses:', data);
      },
    });
    this.getmailofselectedUser();
  }

  getallcategories()
  {
    this.categ_service.getCategories().subscribe({
      next:(data:any)=>{
        // console.log(data);
        this.allCategories = data.data;
        console.log(this.allCategories)
      }
    })
  }

  getmailofselectedUser()
  {
    var user = this.allUsers.find(user=>user.id === this.selectedUser);
    this.selectMail = user.email;
  }
  ngOnInit(): void {
    this.getcategories();
    this.getusers();
    this.getallusers();
    this.getallcategories();
    
  }
  openAddModal()
  {
    this.suggestion = '';
    this.isAddModalOpen.set(true);
  }
  closeAddModal()
  {
    this.suggestion = '';
    this.isAddModalOpen.set(false);
  }
  onSubmitNewSuggestion(form: NgForm)
  {
    this.service.suggestion_email(this.selectMail, this.suggestion).subscribe({
      next:(data:any)=>{
        console.log(data);
        this.closeAddModal();
        this.toast.success("Suggestion sent");
        this.suggestion='';
      }
    })
  }
}