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
    userid:string='';
    newcategory='';
    enablecategoryAdd:boolean=false;
    constructor( private authservice:AUthService, private prodservice: ProductService) {}
  
    ngOnInit(): void {
      this.token = localStorage.getItem("token");
      if (this.token) {
        this.authservice.setRole(this.token);
      }
      this.authservice.role$.subscribe(role=>{
        this.role=role;
        this.isadmin = this.role=='ADMIN';
      })
      this.authservice.userid$.subscribe(userid=>{
        this.userid=userid;
      })
      const storedUserId = localStorage.getItem("userid");
        if (storedUserId) {
          this.userid = storedUserId;
        }
    }
    setenablecategoryAdd()
    {
      this.enablecategoryAdd=true;
    }
    handlecategoryAddrequest()
    {
      this.prodservice.makecategoryaddrequest(this.newcategory).subscribe({
        next:(data:any)=>{
          console.log(data);
        }
      })
      this.closeModal();
      alert("Request made successfully");
    }
    closeModal()
    {
      this.enablecategoryAdd=false;
    } 
}

