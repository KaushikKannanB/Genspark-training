import { Component, inject, ViewEncapsulation } from '@angular/core';
import { AUthService } from '../services/Authentication.service';
import { UserLoginModel } from '../models/UserLoginModel';
import { UserCreate } from '../models/UserCreateModel';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone:true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  encapsulation:ViewEncapsulation.None
})
export class Login {
    private service = inject(AUthService);
    private router = inject(Router);
    user:UserLoginModel=new UserLoginModel();
    username?:number;

    newUser:UserCreate=new UserCreate();
    showSignup = false;
    login()
    {
      this.service.login(this.user).subscribe({
        next:(data:any)=>{
          console.log(data);
          localStorage.setItem("username",data.username);
          localStorage.setItem("token",data.token);

          this.router.navigate(['/product']);
        }
      })
    }
    signup()
    {
      this.service.signup(this.newUser).subscribe({
        next:(data:any)=>{
          console.log(data);
          alert('Sign up successful!');
          this.showSignup=false;
        }
      })
    }
}
