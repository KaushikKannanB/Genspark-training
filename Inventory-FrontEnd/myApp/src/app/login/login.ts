import { Component, inject, ViewEncapsulation } from '@angular/core';
import { UserLoginModel } from '../models/UserLoginModel';
// import { UserService } from '../services/UserService';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AUthService } from '../services/Authentication.service';
import { EmailValidator } from '../misc/EmailValidator';
import { passwordStrengthValidator } from '../misc/PasswordValidator';
import { CommonModule } from '@angular/common';



@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
  encapsulation: ViewEncapsulation.None

})
export class Login {
  private authservice = inject(AUthService);
  private router = inject(Router);
  user:UserLoginModel=new UserLoginModel();
  useremail:string="";
  userid:string="";
  userrole:string="";
  loginform:FormGroup;
  constructor(){
    this.loginform = new FormGroup({
      email : new FormControl(null,[Validators.required,EmailValidator()]),
      password: new FormControl(null, [Validators.required, passwordStrengthValidator()])
    });
  }
  get email() {
    return this.loginform.get('email')!;
  }

  get password() {
    return this.loginform.get('password')!;
  }

  handlelogin()
  {
    this.authservice.login(this.user).subscribe({
      next:(data:any)=>{
        this.useremail=data.email as string;
        this.authservice.getuserfrommail(this.useremail).subscribe({
          next:(data:any)=>{
            this.userid=data.id;
            // console.log(data.id);
            this.authservice.getUserId(this.userid);
            localStorage.setItem("userid",this.userid);
          }
        })

        localStorage.setItem("token", data.token);
        this.authservice.setLoggedIn(true);
        this.authservice.setRole(data.token);
        this.router.navigate(['/home']);

        console.log(this.useremail);
        console.log(data);
        this.userrole=this.authservice.getrolefromtoken(data.token);
        console.log(this.userrole);
      },
      error:(err)=>{
        alert("Invalid credentials")
      }
    })
  }
}