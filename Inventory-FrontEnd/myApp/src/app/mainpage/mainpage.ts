import { Component, inject, OnInit } from '@angular/core';
import { Menu } from "../menu/menu";
import { ProductService } from '../services/Products.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AUthService } from '../services/Authentication.service';

@Component({
  selector: 'app-mainpage',
  imports: [FormsModule, CommonModule,RouterLink],
  templateUrl: './mainpage.html',
  styleUrl: './mainpage.css'
})
export class Mainpage implements OnInit{
    isadmin:boolean=false;
    token:string|null='';
    role:string='';
    constructor( private authservice:AUthService) {}
  
    ngOnInit(): void {
      this.token = localStorage.getItem("token");
      if (this.token) {
        this.authservice.setRole(this.token);
      }
      this.authservice.role$.subscribe(role=>{
        this.role=role;
        this.isadmin = this.role=='ADMIN';
      })
    }
    
      
}

