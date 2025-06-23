import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { UserModel } from '../models/UserModel';
import { FormsModule } from '@angular/forms';
import { ChartConfiguration } from 'chart.js';
// import { NgChartsComponent } from 'ng2-charts';

@Component({
  selector: 'app-user-dashboard',
  imports: [FormsModule],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.css'
})
export class UserDashboard implements OnInit {
  private userservice = inject(UserService);
  Users:UserModel[]=[];
  newUser:UserModel=new UserModel();
  filteredUsers:UserModel[]=[];
  searchGender:string="";
  searchRole:string="";
  searchState:string="";
  genderChartData:number[]=[];
  roleChartData:number[]=[];
  genderChartLabel:string[]=[];
  roleChartLabel:string[]=[];
  ngOnInit(): void {
    this.userservice.getAllUsers().subscribe({
      next:(data:any)=>{
        this.Users = data.users as UserModel[];
        this.filteredUsers = [...this.Users];
        this.processChartData();
      }
    })
  }
  filterUsers()
  {
    this.searchGender = this.searchGender.toLowerCase();
    this.searchRole = this.searchRole.toLowerCase();
    this.searchState = this.searchState.toLowerCase();

    this.filteredUsers = this.Users.filter(user=>{
      return user.gender.toLowerCase()==(this.searchGender) && user.role.toLowerCase().includes(this.searchRole) && user.address.state.toLowerCase().includes(this.searchState)
      
    })
    
    console.log(this.filteredUsers)
  }
  addUser(user:UserModel){
    this.userservice.addUser(this.newUser).subscribe({
      next:(data:any)=>{
        var addedUser = data as UserModel;
        addedUser = { ...data, role: this.newUser.role };
        console.log(addedUser);
        this.Users = [...this.Users,addedUser];
        this.filteredUsers = [...this.filteredUsers, addedUser];
        this.newUser = new UserModel();
      }
    })
  }

  processChartData(){
    const genderMap=new Map<string, number>();
    const roleMap=new Map<string, number>();

    for(var user of this.filteredUsers)
    {
      const gender = user.gender || "Unknown";
      genderMap.set(gender, (genderMap.get(gender) || 0) + 1);

      const role = user.role || "Unknown";
      roleMap.set(role, (roleMap.get(role) || 0) + 1);
    }
    this.genderChartLabel = Array.from(genderMap.keys());
    this.genderChartData = Array.from(genderMap.values());

    this.roleChartLabel = Array.from(roleMap.keys());
    this.roleChartData = Array.from(roleMap.values());

  }
}

