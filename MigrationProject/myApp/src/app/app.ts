import { Component, OnInit } from '@angular/core';
import { Menu } from './menu/menu';
import { RouterOutlet } from '@angular/router';

import { AUthService } from './services/Authentication.service';
import { CommonModule } from '@angular/common';

import { NotExpr } from '@angular/compiler';
import { Login } from './login/login';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [ RouterOutlet,CommonModule, Menu]
})
export class App {
  constructor(public auth: AUthService) {} // Injected as public so it can be accessed in the template
}
