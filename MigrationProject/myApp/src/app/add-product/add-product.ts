import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { ProductAddModel } from '../models/product';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Menu } from '../menu/menu';
import { numberValidator } from '../misc/NumberValidator';

@Component({
  selector: 'app-add-product',
  imports: [FormsModule,CommonModule, ReactiveFormsModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css',
  encapsulation: ViewEncapsulation.None 

})
export class AddProduct implements OnInit {
  private productservice = inject(ProductService);
  private router = inject(Router);
  addproduct:ProductAddModel=new ProductAddModel();
  products:any;
  error:string="";
  categories:any;
  filteredproducts:any;
  productForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    price: new FormControl('', [Validators.required, numberValidator()]),
    categoryName: new FormControl('', [Validators.required]),
    stock: new FormControl('', [Validators.required, numberValidator()]),
    minThreshold: new FormControl('', [Validators.required, numberValidator()])
  });

  get name() { return this.productForm.get('name')!; }
  get description() { return this.productForm.get('description')!; }
  get price() { return this.productForm.get('price')!; }
  get categoryName() { return this.productForm.get('categoryName')!; }
  get stock() { return this.productForm.get('stock')!; }
  get minThreshold() { return this.productForm.get('minThreshold')!; }
  
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
