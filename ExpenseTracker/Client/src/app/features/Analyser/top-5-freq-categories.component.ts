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
  selector: 'app-top-category-freq-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Top 5 Frequent Categories</div>
    <canvas #chart class="w-full h-64"></canvas>
  `
})
export class TopCategoryFreqChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() categories: { categoryName: string; frequency: number }[] = [];
  @ViewChild('chart') chartRef!: ElementRef<HTMLCanvasElement>;
  private chart!: Chart;

  ngAfterViewInit() { this.render(); }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['categories'] && !changes['categories'].firstChange) this.update();
  }
  ngOnDestroy() { this.chart?.destroy(); }

  private render() {
    const labels = this.categories.map(c => c.categoryName);
    const data = this.categories.map(c => c.frequency);
    this.chart = new Chart(this.chartRef.nativeElement, {
      type: 'pie',
      data: { labels, datasets: [{ data, backgroundColor: ['#60A5FA', '#34D399', '#FBBF24', '#F87171', '#A78BFA'] }] },
      options: { responsive: true }
    });
  }

  private update() {
    const labels = this.categories.map(c => c.categoryName);
    const data = this.categories.map(c => c.frequency);
    this.chart.data.labels = labels;
    this.chart.data.datasets[0].data = data;
    this.chart.update();
  }
}