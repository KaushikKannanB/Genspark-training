import { Component } from '@angular/core';
// import { First } from "./first/first";
import { Menu } from './menu/menu';


import { RouterOutlet } from '@angular/router';
// import { UserDashboard } from "./user-dashboard/user-dashboard";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Menu, RouterOutlet]
})
export class App {
  protected title = 'myApp';
}
