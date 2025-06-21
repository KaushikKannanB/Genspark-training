import { HttpClient } from "@angular/common/http"
import { inject } from "@angular/core"
import { UserLoginModel } from "../models/UserLoginModel";
import { jwtDecode } from "jwt-decode";

export class AUthService
{
    private http = inject(HttpClient);
    
    login(user:UserLoginModel)
    {
        return this.http.post('http://localhost:5077/api/authentication/Login',user);
    }
    getrolefromtoken(token:string)
    {
        const decoded: any = jwtDecode(token);
        return decoded.role;
    }
}