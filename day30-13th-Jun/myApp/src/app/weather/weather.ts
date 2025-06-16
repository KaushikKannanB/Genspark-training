import { Component } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { WeatherCard } from "../weather-card/weather-card";

@Component({
  selector: 'app-weather',
  imports: [FormsModule, CommonModule, WeatherCard],
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
  search()
  {
    if(this.city.trim())
    {
      this.weatherservice.setcity(this.city);
    }
  }
}
