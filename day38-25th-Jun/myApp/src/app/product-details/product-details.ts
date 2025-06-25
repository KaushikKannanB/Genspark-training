import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails implements OnInit{
  productid:number=0;
  product:ProductModel=new ProductModel();
  router=inject(ActivatedRoute)
  productservice = inject(ProductService);
  constructor(){

  }
  ngOnInit(): void {
    this.router.params.subscribe(params => {
      this.productid = Number(params["id"]);
      this.loadProduct();
    });

    
  }
  loadProduct() {
    this.productservice.getProduct(this.productid).subscribe({
      next: (data: any) => {
        this.product = data as ProductModel;
        console.log("Loaded product", this.product);
      },
      error: (err) => {
        console.error("Error loading product", err);
      }
    });
  }
  
}
