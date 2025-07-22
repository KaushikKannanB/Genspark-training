import {
  Component,
  Input,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  OnDestroy
} from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-top-category-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Top Categories</div>
    <canvas #categoryChart class="w-full h-64"></canvas>
  `
})
export class TopCategoryChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() categories: { categoryName: string; totalAmount: number }[] = [];
  @ViewChild('categoryChart') chartRef!: ElementRef<HTMLCanvasElement>;

  private chart!: Chart;

  ngAfterViewInit(): void {
    this.renderChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['categories'] && !changes['categories'].firstChange) {
      this.updateChart();
    }
  }

  private renderChart() {
    const data = this.aggregateData();

    const config: ChartConfiguration = {
  type: 'doughnut',
  data: {
    labels: data.labels,
    datasets: [
      {
        label: 'Total Amount',
        data: data.totals,
        backgroundColor: ['#60A5FA', '#F87171', '#34D399', '#FBBF24', '#A78BFA']
      }
    ]
  },
  options: {
    responsive: true,
    maintainAspectRatio: false, // ⚠️ Allow custom height
    plugins: {
      legend: { position: 'bottom' }
    },
    layout: {
      padding: {
        top: 10,
        bottom: 10
      }
    }
  }
};
this.chart = new Chart(this.chartRef.nativeElement, config);
  }

  private updateChart() {
    const data = this.aggregateData();
    if (this.chart) {
      this.chart.data.labels = data.labels;
      this.chart.data.datasets[0].data = data.totals;
      this.chart.update();
    }
  }

  private aggregateData() {
    return {
      labels: this.categories.map(c => c.categoryName),
      totals: this.categories.map(c => c.totalAmount)
    };
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }
}
