import { Component, inject } from '@angular/core';
import { UserService } from '../services/UserService';
import { UserModel } from '../models/UserModel';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  userservice = inject(UserService);
  profileData:UserModel=new UserModel();

  constructor(){
    this.userservice.callGetProfile().subscribe({
      next:(data:any)=>{
        this.profileData = UserModel.fromForm(data);
      }
    })
  }


}
