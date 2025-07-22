import {
  AfterViewInit,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
  ElementRef,
} from '@angular/core';
import Chart from 'chart.js/auto';
import dayjs from 'dayjs';
import ChartDataLabels from 'chartjs-plugin-datalabels';

Chart.register(ChartDataLabels);
@Component({
  selector: 'app-user-spending-trends',
  standalone: true,
  template: `
    <div class="card bg-zinc-900 p-4 shadow-lg rounded-2xl text-white">
      <h2 class="text-lg font-semibold mb-4">User Spending Trends</h2>
      <canvas #lineChartCanvas></canvas>
    </div>
  `,
})
export class UserSpendingTrendsComponent implements AfterViewInit, OnChanges {
  @Input() expenses: any[] = [];
  @Input() filters: {
    categoryId?: string;
    minAmount?: number;
    maxAmount?: number;
    fromDate?: string;
    toDate?: string;
  } = {};
  
  @ViewChild('lineChartCanvas') canvasRef!: ElementRef;
  lineChart: Chart | null = null;

  ngAfterViewInit(): void {
    if (this.expenses.length > 0) {
      setTimeout(() => this.renderCharts(), 0);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['expenses'] && changes['expenses'].currentValue) {
      if (this.lineChart) {
        this.updateCharts();
      } else if (this.canvasRef) {
        this.renderCharts();
      }
    }
  }

  renderCharts() {
    if (this.lineChart) {
      this.lineChart.destroy(); // destroy existing chart
    }

    const chartData = this.getLineChartData();

    this.lineChart = new Chart(this.canvasRef.nativeElement.getContext('2d'), {
      type: 'line',
      data: {
        labels: chartData.labels,
        datasets: [
        {
            label: 'Expense Amount',
            data: chartData.values,
            borderColor: '#38bdf8',
            backgroundColor: 'rgba(26, 43, 51, 0.2)',
            fill: true,
            tension: 0.3,
            pointRadius: 4,             // 👈 Make points visible
            pointHoverRadius: 6,        // 👈 Increase size on hover
            pointBackgroundColor: '#38bdf8',
            pointBorderColor: '#fff',
            pointBorderWidth: 1,
        },
        ],

      },
      options: {
        responsive: true,
        plugins: {
            datalabels: {
            color: '#fff',
            anchor: 'end',
            align: 'top',
            font: {
                weight: 'bold',
                size: 12,
            },
            formatter: (value) => `₹${value}`, // Customize label format
            },
            legend: {
            labels: {
                color: '#fff',
            },
            },
        },
        scales: {
            x: {
            title: {
                display: true,
                text: 'Date',
                color: '#fff',
            },
            ticks: {
                color: '#ccc',
            },
            },
            y: {
            title: {
                display: true,
                text: 'Amount',
                color: '#fff',
            },
            ticks: {
                color: '#ccc',
            },
            },
        },
    }

    });
  }

  updateCharts() {
    if (!this.lineChart) return;
    const chartData = this.getLineChartData();
    this.lineChart.data.labels = chartData.labels;
    this.lineChart.data.datasets[0].data = chartData.values;
    this.lineChart.update();
  }

  getLineChartData() {
    const filteredExpenses = this.expenses.filter(exp => {
        const expenseDate = new Date(exp.expenseDate);
        const from = this.filters.fromDate ? new Date(this.filters.fromDate + 'T00:00:00') : null;
        const to = this.filters.toDate ? new Date(this.filters.toDate + 'T23:59:59') : null;

        return (
        (!this.filters.categoryId || exp.categoryId === this.filters.categoryId) &&
        (!this.filters.minAmount || exp.amount >= this.filters.minAmount) &&
        (!this.filters.maxAmount || exp.amount <= this.filters.maxAmount) &&
        (!from || expenseDate >= from) &&
        (!to || expenseDate <= to)
        );
    });

    // Group by formatted date
    const grouped = filteredExpenses.reduce((acc, exp) => {
        const date = dayjs(exp.expenseDate).format('DD MMM YYYY');
        if (!acc[date]) {
        acc[date] = 0;
        }
        acc[date] += exp.amount;
        return acc;
    }, {} as Record<string, number>);

    // Sort by date
    const sortedDates = Object.keys(grouped).sort((a, b) =>
        dayjs(a, 'DD MMM YYYY').unix() - dayjs(b, 'DD MMM YYYY').unix()
    );

    const labels = sortedDates;
    const values = sortedDates.map(date => grouped[date]);

    console.log('Chart Data:', { labels, values });

    return { labels, values };
    }

}
