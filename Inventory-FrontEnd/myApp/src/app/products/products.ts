import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductAddModel } from '../models/product';
import { StockUpdateModel } from '../models/StockUpdateModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-products',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit{
  private productservice = inject(ProductService);
  products:any;
  filteredproducts:any;
  searchByname:string="";
  searchByStatus:string="";
  searchByMinPrice?:number;
  searchByMaxPrice?:number;
  addproduct:ProductAddModel=new ProductAddModel();
  stockUpd:StockUpdateModel=new StockUpdateModel();
  delete_product:string="";
  ngOnInit(): void 
  {
    this.productservice.getallproducts().subscribe({
      next:(data:any)=>{
        this.products = data;
        data.forEach((product:any) => {
          this.productservice.getstocksbyproductname(product.name).subscribe({
            next:(data:any)=>{
              product.stock=data.stock;
              product.minThreshold=data.minThreshold;
              console.log(data);
            }
          });
        });
        this.filteredproducts=this.products;
      }
    })
  }
  addProduct()
  {
    this.productservice.addproduct(this.addproduct).subscribe({
      next:(data:any)=>{
        console.log(data);
      }
    })
  }
  handledelete(prodName:string)
  {
    this.productservice.deleteproduct(prodName).subscribe({
      next:(data:any)=>{
        console.log(data);
      }
    })
  }
  handlestockupdate()
  {
    this.productservice.updatestock(this.stockUpd).subscribe({
      next:(data:any)=>{
        console.log(data);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }
  filterProducts() {
    const searchName = this.searchByname?.toUpperCase() || '';
    const searchStatus = this.searchByStatus?.toUpperCase() || '';

    this.filteredproducts = this.products.filter((product: any) => {
      const matchName = product.name?.toUpperCase().includes(searchName);
     const matchStatus = searchStatus
      ? product.status?.toUpperCase() === searchStatus
      : true;
      const matchMinPrice = this.searchByMinPrice != null ? product.price >= this.searchByMinPrice : true;
      const matchMaxPrice = this.searchByMaxPrice != null ? product.price <= this.searchByMaxPrice : true;

      return matchName && matchStatus && matchMinPrice && matchMaxPrice;
    });
  }
}
