import { Component, OnInit } from '@angular/core';
import { Menu } from './menu/menu';
import { RouterOutlet } from '@angular/router';
import { NotificationService } from './services/Notification.service';
import { AUthService } from './services/Authentication.service';
import { CommonModule } from '@angular/common';
import { Mainpage } from './mainpage/mainpage';
import { Login } from './login/login';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [ Menu, RouterOutlet,CommonModule,Login]
})
export class App implements OnInit {
  protected title = 'myApp';
  isloggedin:boolean=false;
  constructor(private notifyService: NotificationService, private authservice:AUthService) {}

  ngOnInit() {
    this.notifyService.startConnection();
    console.log('[App] Connection started. Messages length:', this.notifyService.messages.length);
    this.authservice.isLoggedIn$.subscribe(isLoggedIn=>{
      this.isloggedin=isLoggedIn;
    })
    
  }
}
