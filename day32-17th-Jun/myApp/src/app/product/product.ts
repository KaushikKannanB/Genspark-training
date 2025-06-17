import { Component, inject, Input, EventEmitter, Output } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { CurrencyPipe } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe, RouterOutlet],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
@Input() product:ProductModel|null = new ProductModel();
@Output() addToCart:EventEmitter<Number> = new EventEmitter<Number>();
private productService = inject(ProductService);
private router = inject(Router);
handleBuyClick(pid:Number|undefined){
  if(pid)
  {
      this.addToCart.emit(pid);
  }
}
handleViewMoreDetails(pid:number|undefined)
{
  if(pid)
  {
    this.router.navigateByUrl(`/products/${pid}`);
  }
}
constructor(){
    // this.productService.getProduct(1).subscribe(
    //   {
    //     next:(data)=>{
     
    //       this.product = data as ProductModel;
    //       console.log(this.product)
    //     },
    //     error:(err)=>{
    //       console.log(err)
    //     },
    //     complete:()=>{
    //       console.log("All done");
    //     }
    //   })
}

}
