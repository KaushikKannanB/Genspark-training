import { Component } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-weather',
  imports: [FormsModule,CommonModule],
  templateUrl: './weather.html',
  styleUrl: './weather.css'
})
export class Weather {
  city='London';
  weatherData:any;
  error:string|null="";
  constructor(private weatherservice: WeatherService)
  {
    
  }
  fetchWeather()
  {
    this.weatherservice.getWeatherByCity(this.city).subscribe({
      next:(data)=>{
        this.weatherData=data;
        this.error=null;
      },
      error:(err)=>{
        this.error = err.message;
        this.weatherData = null;
      }
    });
  }
}
