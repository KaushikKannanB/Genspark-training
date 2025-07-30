import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../services/Products.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products implements OnInit {
  private service = inject(ProductService);
  products: any[] = [];

  loadproducts() {
    this.service.getallproducts().subscribe({
      next: (data: any) => {
        console.log(data);
        this.products = data;
      }
    });
  }

  ngOnInit(): void {
    this.loadproducts();
  }
}
