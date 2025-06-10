import { Component } from '@angular/core';
import { First } from "./first/first";
import { CustomerComponent } from './customer-component/customer-component';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, CustomerComponent]
})
export class App {
  protected title = 'myApp';
}