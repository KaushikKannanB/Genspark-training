import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NotificationService } from '../services/Notification.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Menu } from '../menu/menu';


@Component({
  selector: 'app-notification',
  templateUrl: './notifications.html',
  imports: [FormsModule,CommonModule],
  styleUrls:['./notifications.css'],
  encapsulation: ViewEncapsulation.None

})
export class Notifications implements OnInit {

  constructor(public notifyService: NotificationService) {}

  ngOnInit(): void {
    // this.notifyService.startConnection();
    // console.log(this.notifyService.messages);
  }
  getNotificationClass(msg: string): string {
  const m = msg.toLowerCase();

  if (m.includes('added') && m.includes('product')) return 'product';
  if (m.includes('stock')) return 'restock';
  if (m.includes('request')) return 'request';
  if (m.includes('category') && m.includes('added')) return 'category';

  return 'default';
}

  
}