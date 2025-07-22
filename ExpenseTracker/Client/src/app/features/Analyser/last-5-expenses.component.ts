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
  selector: 'app-last-expenses-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Last 5 Expenses</div>
    <canvas #chart class="w-full h-64"></canvas>
  `
})
export class LastExpensesChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() expenses: { amount: number; description: string; date: string }[] = [];
  @ViewChild('chart') chartRef!: ElementRef<HTMLCanvasElement>;
  private chart!: Chart;

  ngAfterViewInit() { this.render(); }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['expenses'] && !changes['expenses'].firstChange) this.update();
  }
  ngOnDestroy() { this.chart?.destroy(); }

  private render() {
    const labels = this.expenses.map(e => e.description);
    const data = this.expenses.map(e => e.amount);
    this.chart = new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: { labels, datasets: [{ label: 'Amount', data, backgroundColor: '#34D399' }] },
      options: { responsive: true, plugins: { legend: { display: false } } }
    });
  }

  private update() {
    const labels = this.expenses.map(e => e.description);
    const data = this.expenses.map(e => e.amount);
    this.chart.data.labels = labels;
    this.chart.data.datasets[0].data = data;
    this.chart.update();
  }
}
