import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, catchError, throwError } from "rxjs";
import { WeatherResponse } from "../models/weathermodel";

export class WeatherService
{
    private apikey='dcd3b9e633d753b4407df80e818b26e0';
    private apiurl = 'https://api.openweathermap.org/data/2.5/weather';

    private http = inject(HttpClient);

    private citysubject = new BehaviorSubject<string>('London');
    city$ = this.citysubject.asObservable();

    private weathersubject = new BehaviorSubject<WeatherResponse|null>(null);
    weather$ = this.weathersubject.asObservable();

    setcity(city:string)
    {
        this.citysubject.next(city);
        this.getWeatherByCity(city).subscribe({
            next:(data)=>{
                this.weathersubject.next(data);
            },
            error:(err)=>{
                this.weathersubject.error(err);
            }
        });
    }
    getWeatherByCity(city:string)
    {
        const url = `${this.apiurl}?q=${city}&appid=${this.apikey}&units=metric`;
        return this.http.get<WeatherResponse>(url).pipe(catchError(this.handleerror));
    }
    handleerror(error:HttpErrorResponse)
    {
        let errorMessage = 'Something went wrong!';
        if (error.status === 404) {
        errorMessage = 'City not found.';
        } else if (error.error instanceof ErrorEvent) {
        errorMessage = `Client error: ${error.error.message}`;
        } else {
        errorMessage = `Server error: ${error.message}`;
        }
        return throwError(() => new Error(errorMessage));
    }
}