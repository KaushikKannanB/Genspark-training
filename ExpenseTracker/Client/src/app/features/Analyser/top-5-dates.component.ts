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

@Component({
  selector: 'app-top-dates-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Top 5 Spending Days</div>
    <canvas #chart class="w-full h-64"></canvas>
  `
})
export class TopDatesChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() dates: { date: string; totalAmount: number }[] = [];
  @ViewChild('chart') chartRef!: ElementRef<HTMLCanvasElement>;
  private chart!: Chart;

  ngAfterViewInit() { this.render(); }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['dates'] && !changes['dates'].firstChange) this.update();
  }
  ngOnDestroy() { this.chart?.destroy(); }

  private render() {
    const labels = this.dates.map(d => new Date(d.date).toLocaleDateString());
    const data = this.dates.map(d => d.totalAmount);
    this.chart = new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: { labels, datasets: [{ label: 'Total Spent', data, backgroundColor: '#FBBF24' }] },
      options: { responsive: true, plugins: { legend: { display: false } } }
    });
  }

  private update() {
    const labels = this.dates.map(d => new Date(d.date).toLocaleDateString());
    const data = this.dates.map(d => d.totalAmount);
    this.chart.data.labels = labels;
    this.chart.data.datasets[0].data = data;
    this.chart.update();
  }
}