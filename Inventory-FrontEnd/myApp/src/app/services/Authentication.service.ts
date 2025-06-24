import { HttpClient, HttpHeaders } from "@angular/common/http"
import { inject } from "@angular/core"
import { UserLoginModel } from "../models/UserLoginModel";
import { jwtDecode } from "jwt-decode";
import { BehaviorSubject } from "rxjs";

export class AUthService
{
    private http = inject(HttpClient);
    token:string|null="";
    headers:any;
    private isLoggedInSubject = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
    isLoggedIn$ = this.isLoggedInSubject.asObservable();
    role$ = new BehaviorSubject<string>('');
    setRole(token:string)
    {
        const role = this.getrolefromtoken(token);
        this.role$.next(role);
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
        return this.http.post('http://localhost:5077/api/authentication/Login',user);
    }
    logout()
    {
        this.authorization();
        const token = localStorage.getItem("token") as string;
        localStorage.removeItem("token");
        return this.http.post('http://localhost:5077/api/authentication/logout',token,{headers:this.headers})
    }
    getrolefromtoken(token:string|any)
    {
        const decoded: any = jwtDecode(token);
        return decoded.role;
    }
}