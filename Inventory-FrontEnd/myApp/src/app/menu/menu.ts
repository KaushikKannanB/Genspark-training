import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../services/Notification.service';
import { AUthService } from '../services/Authentication.service';
import { AdminManagerService } from '../services/AdminManager.service';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { passwordStrengthValidator } from '../misc/PasswordValidator';

@Component({
  selector: 'app-menu',
  imports: [RouterLink, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
  encapsulation: ViewEncapsulation.None

})
export class Menu implements OnInit {
  logout:string=""
  role:string='';
  newpass:string='';
  enablechangepass:boolean=false;
  loginform:FormGroup;


  constructor(private fb: FormBuilder, public notifyService: NotificationService, public authservice:AUthService, private router :Router, private admmanservice:AdminManagerService) {
    this.loginform = new FormGroup({
      password: new FormControl(null, [Validators.required, passwordStrengthValidator()])
    });
  }
  ngOnInit(): void {
    this.authservice.role$.subscribe(role=>{
      this.role=role;
    });
    
  }
  get password() {
    return this.loginform.get('password')!;
  }
  
  handlelogout()
  {
    this.authservice.logout().subscribe({
      next:(data:any)=>{
        // alert(data);
      },
      error:(err)=>{
        // console.log(err.error.text);
        this.logout = err.error.text;
        console.log(this.logout);
      }
    })
    alert("Succesful Logout");
    this.authservice.setLoggedIn(false);
    // this.router.navigate(["login"]);
  }
  setenablechangepass()
  {
    this.enablechangepass=true;
  }
  handlechangepassword()
  {
    if(this.role=='ADMIN')
    {
      this.admmanservice.adminchangpassword(this.newpass).subscribe({
        next:(data:any)=>{
          console.log(data);
          this.closeModal();
        }
      })
    }
    else
    {
      this.admmanservice.managerchangpassword(this.newpass).subscribe({
        next:(data:any)=>{
          console.log(data);
          alert("Password Successfully changed!");
          this.closeModal();
        }
      })
    }
  }
  closeModal()
  {
    this.enablechangepass=false;
  }
}
