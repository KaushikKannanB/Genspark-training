import { Component, inject, OnInit } from '@angular/core';
import { Menu } from "../menu/menu";
import { ProductService } from '../services/Products.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-mainpage',
  imports: [FormsModule, CommonModule,RouterLink],
  templateUrl: './mainpage.html',
  styleUrl: './mainpage.css'
})
export class Mainpage{
  
}
