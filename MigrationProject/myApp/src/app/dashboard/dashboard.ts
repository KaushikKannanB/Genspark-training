declare const html2pdf: any;

import { Component, OnInit } from '@angular/core';
import { ChartConfiguration, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { ProductService } from '../services/Products.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [BaseChartDirective, FormsModule, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  selectedproduct: string = '';
  selectedproductstocklogs: any[] = [];
  allproducts: any[] = [];
  allcategories:any;
  allstocklogs:any;
  report:any;
  barChartType: ChartType = 'line';
  private usedHues: number[] = [];
  topProductsChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: []
  };
  barChartData: ChartConfiguration['data'] = {
    labels: [],
    datasets: []
  };

  barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false
  };

  categoryChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: []
  };

categoryChartType: ChartType = 'pie';
  constructor(private prodservice: ProductService) {}

  ngOnInit(): void {
    this.loadallproducts();
    this.loadallcategories();
    this.loadallstocklogs();
  }

  loadallproducts() {
    this.prodservice.getallproducts().subscribe({
      next: (data: any) => {
        this.allproducts = data.sort((a: any, b: any) =>
          a.name.localeCompare(b.name)
        );
        // this.allproducts.sort();
        console.log(data);
      }
    });
  }
  loadallcategories()
  {
    this.prodservice.getallcategories().subscribe({
      next:(data:any)=>{
        this.allcategories=data;
        console.log(data);
        this.generateProductCategoryChart();
      }
    })
  }
  loadallstocklogs()
  {
    this.prodservice.getallstocklogs().subscribe({
      next:(data:any)=>{
        this.allstocklogs=data;
        console.log(this.allstocklogs);
        this.generateTopProductsSoldChart();
      }
    })
  }
  setselectedproduct() {
    if (!this.selectedproduct) return;

    this.getstockupdatesforproduct(this.selectedproduct);
    this.getproductreport(this.selectedproduct);
  }

  getstockupdatesforproduct(prod: string) {
    this.prodservice.getstocklogsbyproductname(prod).subscribe({
      next: (data: any) => {
        this.selectedproductstocklogs = data;
        this.generateLineChartFromLogs();
      }
    });
  }

  getproductreport(prod: string) {
    this.prodservice.getproducsummary(prod).subscribe({
      next: (data: any) => {
        this.report=data;
        console.log(this.report);
      }
    });
  }
  getRandomColor(): string {
  let hue: number;
  let attempts = 0;

  do {
    hue = Math.floor(Math.random() * 360);
    attempts++;
  } while (this.usedHues.some(h => Math.abs(h - hue) < 25) && attempts < 100); // avoid close hues

  this.usedHues.push(hue);
  return `hsl(${hue}, 70%, 60%)`;
}

  generateLineChartFromLogs() {
    if (!this.selectedproductstocklogs.length) return;

    const labels = this.selectedproductstocklogs.map((log, index) =>
      log.updatedAt ? new Date(log.updatedAt).toLocaleString() : `Update ${index + 1}`
    );

    const stockData = this.selectedproductstocklogs.map(log => log.newStock);

    this.barChartData = {
      labels,
      datasets: [
        {
          data: stockData,
          label: 'Stock Level',
          fill: false,
          borderColor: '#7e5bef',
          backgroundColor: '#7e5bef',
          tension: 0.4,
          pointBackgroundColor: '#e26dff',
          pointBorderColor: '#fff',
          pointRadius: 5
        }
      ]
    };

    this.barChartType = 'line';
  }
  generateProductCategoryChart() {
    if (!this.allcategories.length || !this.allproducts.length) return;
    this.usedHues = [];
    const labels: string[] = [];
    const counts: number[] = [];
    const colors: string[] = [];

    for (let cat of this.allcategories) {
      const productCount = this.allproducts.filter(p => p.categoryId === cat.id).length;
      if (productCount > 0) {
        labels.push(cat.categoryName);
        counts.push(productCount);
        colors.push(this.getRandomColor());
      }
    }

    this.categoryChartData = {
      labels,
      datasets: [
        {
          data: counts,
          backgroundColor: colors,
          borderColor: '#fff',
          borderWidth: 1
        }
      ]
    };
  }
  downloadPDF() {
    const element = document.getElementById('pdfSection');
    if (!element) return;

    const options = {
      margin:       0.5,
      filename:     `${this.selectedproduct}-report.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'letter', orientation: 'portrait' }
    };

    html2pdf().from(element).set(options).save();
  }
  generateTopProductsSoldChart() {
  if (!this.allstocklogs?.length || !this.allproducts?.length) return;
  this.usedHues = [];
  const stockOutCounts: { [prodName: string]: number } = {};

  for (let log of this.allstocklogs) {
    if (log.oldStock > log.newStock) {
      const prod = this.allproducts.find(p => p.inventoryId === log.inventoryId);
      if (prod) {
        stockOutCounts[prod.name] = (stockOutCounts[prod.name] || 0) + log.oldStock-log.newStock;
      }
    }
  }

  const sorted = Object.entries(stockOutCounts)
    .sort((a, b) => b[1] - a[1])
    .slice(0, 5);

  const labels = sorted.map(([name]) => name);
  const counts = sorted.map(([, count]) => count);
  const colors = labels.map(() => this.getRandomColor());

  this.topProductsChartData = {
    labels,
    datasets: [
      {
        label: 'Stock Out Count',
        data: counts,
        backgroundColor: colors
      }
    ]
  };
}


}
