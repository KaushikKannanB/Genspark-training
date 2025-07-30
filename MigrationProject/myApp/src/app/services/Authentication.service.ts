import { HttpClient, HttpHeaders } from "@angular/common/http"
import { inject } from "@angular/core"
import { BehaviorSubject } from "rxjs";
import { UserLoginModel } from "../models/UserLoginModel";
import { UserCreate } from "../models/UserCreateModel";

export class AUthService
{
    private http = inject(HttpClient);
    token:string|null="";
    headers:any;
    private isLoggedInSubject = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
    isLoggedIn$ = this.isLoggedInSubject.asObservable();

    userid$ = new BehaviorSubject<string>('');
    getUserId(id:string)
    {
        this.userid$.next(id);
    }
    setLoggedIn(value: boolean) {
        this.isLoggedInSubject.next(value);
    }
    authorization()
    {
        this.token = localStorage.getItem('token');

        this.headers = new HttpHeaders({
        'Authorization': `Bearer ${this.token}`
        });
    }
    login(user:UserLoginModel)
    {
        return this.http.post('http://localhost:5048/api/authentication/login',user);
    }
    logout()
    {
        this.authorization();
        const token = localStorage.getItem("token") as string;
        localStorage.removeItem("token");
        localStorage.removeItem("userid");
    }
    signup(user:UserCreate)
    {
        return this.http.post('http://localhost:5048/api/authentication/signup',user)
    }
    isLoggedIn(): boolean {
        return !!localStorage.getItem('token'); // or your login check logic
    }
}