import { Component, OnInit } from '@angular/core';
import { Menu } from './menu/menu';
import { RouterOutlet } from '@angular/router';
import { NotificationService } from './services/Notification.service';
import { AUthService } from './services/Authentication.service';
import { CommonModule } from '@angular/common';
import { Mainpage } from './mainpage/mainpage';
import { Login } from './login/login';
import { NotExpr } from '@angular/compiler';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [ RouterOutlet,CommonModule,Login, Menu]
})
export class App implements OnInit {
  protected title = 'myApp';
  isloggedin:boolean=false;
  isadmin:boolean=false;
  token:string|null='';
  tokenstring:string='';
  role:string='';
  constructor(private notifyService: NotificationService, private authservice:AUthService) {}

  ngOnInit() {
    this.notifyService.startConnection();
    console.log('[App] Connection started. Messages length:', this.notifyService.messages.length);
    this.authservice.isLoggedIn$.subscribe(isLoggedIn=>{
      this.isloggedin=isLoggedIn;
    })
    if(this.isloggedin)
    {
    this.tokenstring = this.token?this.token:"";
    console.log(this.tokenstring);
    this.authservice.getrolefromtoken(this.tokenstring).subscribe({
      next:(data:any)=>{
        this.role=data;
      }
    });

    }
    
  }
}
