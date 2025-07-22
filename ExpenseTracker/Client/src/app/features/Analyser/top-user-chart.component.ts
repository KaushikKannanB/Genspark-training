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
  selector: 'app-top-user-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Top Users by Expense</div>
    <canvas #userChart class="w-full h-64"></canvas>
  `
})
export class TopUserChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() users: { userName: string; totalExpenses: number }[] = [];
  @ViewChild('userChart') chartRef!: ElementRef<HTMLCanvasElement>;

  private chart!: Chart;

  ngAfterViewInit(): void {
    this.renderChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['users'] && !changes['users'].firstChange) {
      this.updateChart();
    }
  }

  private renderChart() {
    const data = this.aggregateData();

    const config: ChartConfiguration = {
      type: 'bar',
      data: {
        labels: data.labels,
        datasets: [
          {
            label: 'Total Expenses',
            data: data.totals,
            backgroundColor: '#60A5FA'
          }
        ]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false }
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: { stepSize: 100 }
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
      labels: this.users.map(u => u.userName),
      totals: this.users.map(u => u.totalExpenses)
    };
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }
}
