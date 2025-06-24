import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { ProductAddModel } from '../models/product';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Menu } from '../menu/menu';

@Component({
  selector: 'app-add-product',
  imports: [FormsModule,CommonModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css'
})
export class AddProduct implements OnInit {
  private productservice = inject(ProductService);
  private router = inject(Router);
  addproduct:ProductAddModel=new ProductAddModel();
  products:any;
  error:string="";
  categories:any;
  filteredproducts:any;
  ngOnInit(): void {
    this.loadProducts();
    this.getallcategories();
  }
  loadProducts() {
    this.productservice.getallproducts().subscribe({
      next: (data: any) => {
        this.products = data;
        // console.log(data)
        data.forEach((product: any) => {
          this.productservice.getstocksbyproductname(product.name).subscribe({
            next: (data: any) => {
              product.stock = data.stock;
              product.minThreshold = data.minThreshold;
              // console.log(data);
            }
          });
          this.productservice.getcategorybyId(product.categoryId).subscribe({
            next:(data:any)=>{
              product.category = data.categoryName;
            }
          })
        });
        // this.filteredproducts = this.products;
      }
    });
  }
  addProduct()
  {
    this.productservice.addproduct(this.addproduct).subscribe({
      next:(data:any)=>{
        console.log(data);
        alert("Product added Successfully");
        this.router.navigate(["products"]);
      },
      error:(err)=>{
        console.log(err);
        alert("Invalid Product Add Request: "+err.error.text);
      }
    })
  }
  getallcategories()
  {
    this.productservice.getallcategories().subscribe({
      next:(data:any)=>{
        this.categories=data;
      }
    })
  }
  filterProducts() {
    const searchName = this.addproduct.name?.toUpperCase() || '';
    
    this.filteredproducts = this.products.filter((product: any) => {
      const matchName = product.name?.toUpperCase().includes(searchName);
     
      return matchName;
    
    });
    if(this.addproduct.name=='')
    {
      this.filteredproducts=null;
    }
    if(this.filteredproducts.length==0)
    {
      this.filteredproducts=null;
    }
  }
}
