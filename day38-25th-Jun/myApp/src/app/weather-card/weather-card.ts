import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { WeatherResponse } from "../models/weathermodel";

@Component({
  selector: 'app-weather-card',
  imports: [CommonModule, AsyncPipe],
  templateUrl: './weather-card.html',
  styleUrl: './weather-card.css'
})
export class WeatherCard implements OnInit {
  weather$: Observable<WeatherResponse | null>;
  error: string | null = null;

  constructor(private weatherservice: WeatherService) {
    this.weather$ = this.weatherservice.weather$;
  }

  ngOnInit() {
    

    this.weather$.subscribe({
      error: (err:any) => {
        this.error = err.message;
      }
    });
  }
}