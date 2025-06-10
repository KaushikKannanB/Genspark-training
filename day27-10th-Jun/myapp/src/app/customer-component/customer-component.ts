import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-component',
  imports: [CommonModule],
  templateUrl: './customer-component.html',
  styleUrls: ['./customer-component.css']
})
export class CustomerComponent {
  customer={
    id:1, name:"Kaushik",email:"kaushikkannan02@gmail.com", mobile:8248768514
  };
  cartcount:number=0;
  products = [
    {
      id: 1,
      name: 'Wireless Headphones',
      price: 1999,
      image: 'https://via.placeholder.com/150?text=Headphones'
    },
    {
      id: 2,
      name: 'Smart Watch',
      price: 3499,
      image: 'https://via.placeholder.com/150?text=Smart+Watch'
    },
    {
      id: 3,
      name: 'Bluetooth Speaker',
      price: 1499,
      image: 'https://via.placeholder.com/150?text=Speaker'
    }
  ];
  showdetails:boolean=false;
  constructor(){

  }
  ViewCustomerDetails(){
    this.showdetails=true;
  }
  addToCart(){
    this.cartcount++;
  }
}
