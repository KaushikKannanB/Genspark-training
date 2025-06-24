import { Component, inject } from '@angular/core';
import { UserLoginModel } from '../models/UserLoginModel';
// import { UserService } from '../services/UserService';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AUthService } from '../services/Authentication.service';



@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private authservice = inject(AUthService);
  private router = inject(Router);
  user:UserLoginModel=new UserLoginModel();
  useremail:string="";
  userrole:string="";
  handlelogin()
  {
    this.authservice.login(this.user).subscribe({
      next:(data:any)=>{
        this.useremail=data.email as string;
        localStorage.setItem("token", data.token);
        this.authservice.setLoggedIn(true);
        // this.router.navigate(['/home']);
        console.log(this.useremail);
        console.log(data);
        this.userrole=this.authservice.getrolefromtoken(data.token);
        console.log(this.userrole);
      }
    })
  }
}