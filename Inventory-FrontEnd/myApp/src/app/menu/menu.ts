import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../services/Notification.service';
import { AUthService } from '../services/Authentication.service';

@Component({
  selector: 'app-menu',
  imports: [RouterLink, CommonModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css'
})
export class Menu {
  logout:string=""
  constructor(public notifyService: NotificationService, public authservice:AUthService, private router :Router) {
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
}
