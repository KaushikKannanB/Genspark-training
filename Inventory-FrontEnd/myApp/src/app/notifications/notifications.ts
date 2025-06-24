import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/Notification.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Menu } from '../menu/menu';


@Component({
  selector: 'app-notification',
  templateUrl: './notifications.html',
  imports: [FormsModule,CommonModule],
  styleUrls:['./notifications.css']
})
export class Notifications implements OnInit {

  constructor(public notifyService: NotificationService) {}

  ngOnInit(): void {
    // this.notifyService.startConnection();
    // console.log(this.notifyService.messages);
  }

  
}