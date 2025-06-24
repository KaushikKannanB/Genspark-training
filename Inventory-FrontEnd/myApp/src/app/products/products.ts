import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductAddModel } from '../models/product';
import { StockUpdateModel } from '../models/StockUpdateModel';
import { CommonModule } from '@angular/common';
import { Menu } from '../menu/menu';

@Component({
  selector: 'app-products',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit{
  private productservice = inject(ProductService);
  products:any;
  categories:any;
  filteredproducts:any;
  searchByname:string="";
  searchByStatus:string="";
  searchByMinPrice?:number;
  searchByMaxPrice?:number;
  searchByCategory:string="";
  stockUpd:StockUpdateModel=new StockUpdateModel();
  delete_product:string="";
  enableStockUpd:boolean=false;

  enableProdUpd:boolean=false;
  selectedProdName:string="";
  fieldUpdate:string="";
  updateField:'description'|'price'|null=null;
  newValue:string|number='';
  ngOnInit(): void 
  {
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
        this.filteredproducts = this.products;
      }
    });
  }

  
  handledelete(prodName:string)
  {
    this.productservice.deleteproduct(prodName).subscribe({
      next:(data:any)=>{
        // console.log(data);
      }
    })
  }

  
  enableUpdateProduct(prodname:string)
  {
    this.enableProdUpd=true;
    this.selectedProdName=prodname;
  }
  selectField(field:'description'|'price')
  {
    this.updateField=field;
    this.newValue='';
  }
  submitUpdate()
  {
    if(!this.updateField)
      return;

    if(this.updateField=='description')
    {
      const payload = {
        productName: this.selectedProdName.toUpperCase(),
        newDescription: this.newValue
      };

      this.productservice.updateproddescription(payload).subscribe({
        next:(data:any)=>{
          console.log(data);
        }
      })
    }
    else
    {
      const payload = {
        productName: this.selectedProdName.toUpperCase(),
        newprice: this.newValue
      };

      this.productservice.updateprodprice(payload).subscribe({
        next:(data:any)=>{
          console.log(data);
        }
      })
    }
    this.closeUpdateModal();
    
  }
  closeUpdateModal()
  {
    this.enableProdUpd=false;
    this.updateField=null;
    this.loadProducts();
  }
  enableUpdateStock(prodname:string)
  {
    this.stockUpd.ProductName=prodname.toUpperCase();
    this.enableStockUpd=true;
  }
  handlestockupdate()
  {
    this.productservice.updatestock(this.stockUpd).subscribe({
      next:(data:any)=>{
        // console.log(data);
      },
      error:(err)=>{
        alert("Invalid Update Request");
      }
    })
    this.closeModal();
    this.stockUpd=new StockUpdateModel();
    // this.loadProducts();

  }
  closeModal() {
    this.enableStockUpd = false;
    this.loadProducts();
  }
  filterProducts() {
    const searchName = this.searchByname?.toUpperCase() || '';
    const searchStatus = this.searchByStatus?.toUpperCase() || '';
    const searchcatgegory = this.searchByCategory?.toUpperCase() || '';

    this.filteredproducts = this.products.filter((product: any) => {
      const matchName = product.name?.toUpperCase().includes(searchName);
     const matchStatus = searchStatus
      ? product.status?.toUpperCase() === searchStatus
      : true;
     const matchCategory = searchcatgegory?product.category?.toUpperCase()===searchcatgegory:true;
      const matchMinPrice = this.searchByMinPrice != null ? product.price >= this.searchByMinPrice : true;
      const matchMaxPrice = this.searchByMaxPrice != null ? product.price <= this.searchByMaxPrice : true;
      
      return matchName && matchStatus && matchMinPrice && matchMaxPrice && matchCategory;
    });
  }
  getallcategories()
  {
    this.productservice.getallcategories().subscribe({
      next:(data:any)=>{
        this.categories=data;
      }
    })
  }
}
