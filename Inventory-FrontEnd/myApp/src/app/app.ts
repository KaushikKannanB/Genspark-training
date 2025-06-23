import { Component, OnInit } from '@angular/core';
import { Menu } from './menu/menu';
import { RouterOutlet } from '@angular/router';
import { NotificationService } from './services/Notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Menu, RouterOutlet]
})
export class App implements OnInit {
  protected title = 'myApp';

  constructor(private notifyService: NotificationService) {}

  ngOnInit() {
    this.notifyService.startConnection();
    console.log('[App] Connection started. Messages length:', this.notifyService.messages.length);
  }
}
