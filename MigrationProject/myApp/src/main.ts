import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';


import {
  Chart,
  BarController,
  BarElement,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
  Title,LineController, LineElement,
  PointElement, ArcElement, DoughnutController,
  PieController, RadarController
} from 'chart.js';


Chart.register(
  BarController,
  BarElement,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
  Title,LineController, LineElement,
  PointElement, ArcElement, DoughnutController,
  PieController, RadarController
);


bootstrapApplication(App, appConfig)
  .catch(err => console.error(err));
