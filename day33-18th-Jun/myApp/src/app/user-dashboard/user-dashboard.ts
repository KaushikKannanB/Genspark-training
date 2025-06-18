import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { UserModel } from '../models/UserModel';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-dashboard',
  imports: [FormsModule],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.css'
})
export class UserDashboard implements OnInit {
  private userservice = inject(UserService);
  Users:UserModel[]=[];
  filteredUsers:UserModel[]=[];
  searchGender:string="";
  searchRole:string="";
  searchState:string="";
  ngOnInit(): void {
    this.userservice.getAllUsers().subscribe({
      next:(data:any)=>{
        this.Users = data.users as UserModel[];
        this.filteredUsers = [...this.Users];
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

}

