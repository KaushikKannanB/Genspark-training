import { inject } from "@angular/core";
import { HttpService } from "@core/services/http.service";
import { HttpClient } from "@microsoft/signalr";

export class AnalyserService
{
    private http = inject(HttpService);
    endpoint_suggestion_email = '/Analyser/send-suggestion-mail';

    gettopcategories()
    {
        return this.http.get('/Analyser/Get-top5-categories');
    }

    gettopusers()
    {
        return this.http.get('/Analyser/Get-top-users');
    }
    getallusers()
    {
        return this.http.get('/Analyser/Get-all-users');
    }
    getexpensebyid(id:string)
    {
        return this.http.get(`/Analyser/Get-Expenses-By-User-Id?userid=${id}`);
    }

    getexpenses_top5_byid(id: string)
    {
        return this.http.get(`/Analyser/Get-top5-Days-expenses-User?userid=${id}`);
    }

    getexpenses_latest5_byid(id:string)
    {
        return this.http.get(`/Analyser/Get-latest5-expenses-User?userid=${id}`);
    }

    gettop5categories_by_user(id:string)
    {
        return this.http.get(`/Analyser/Get-top5-categories-User-by-amount?userid=${id}`);
    }

    gettop5categories_by_user_freq(id:string)
    {
        return this.http.get(`/Analyser/Get-top5-categories-User-by-frequency?userid=${id}`);
    }

    suggestion_email(id: string, content:string) {
        return this.http.post(`${this.endpoint_suggestion_email}?email=${id}&content=${content}`, null);
    }

    
}