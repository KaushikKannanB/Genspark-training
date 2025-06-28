import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { UserCreate } from "../models/UserCreateModel";


export class AdminManagerService
{
    private http = inject(HttpClient);
    token:string|null="";
    headers:any;
    authorization()
    {
        this.token = localStorage.getItem('token');

        this.headers = new HttpHeaders({
        'Authorization': `Bearer ${this.token}`
        });
    }
    getalladmins()
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/admin/Get-All-Admins',{headers:this.headers});
    }
    getallmanagers()
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/admin/Get-All-Managers',{headers:this.headers});
    }
    getallcategoryrequests()
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/admin/Get-All-Category-Requests',{headers:this.headers});
    }
    createAdmin(user:UserCreate)
    {
        this.authorization();
        return this.http.post('http://localhost:5077/api/admin/Add-Admin',user,{headers:this.headers});
    }
    createManager(user:UserCreate)
    {
        this.authorization();
        return this.http.post('http://localhost:5077/api/admin/Add-Manager',user,{headers:this.headers});
    }
    addCategory(cat:string)
    {
        this.authorization();
        return this.http.post(`http://localhost:5077/api/admin/Add-Category?Category=${cat}`,null,{headers:this.headers});
    }
    handleadminreport(id:string)
    {
        this.authorization();
        return this.http.get(`http://localhost:5077/api/admin/Get-Admin-Report?id=${id}`,{headers:this.headers});
    }
    handlemanagerreport(id:string)
    {
        this.authorization();
        return this.http.get(`http://localhost:5077/api/admin/Get-Manager-Report?id=${id}`,{headers:this.headers});
    }
    adminchangpassword(pwd:string)
    {
        this.authorization();
        return this.http.put(`http://localhost:5077/api/admin/Change-Password-Admin?NewPassword=${pwd}`,null,{headers:this.headers, responseType:'text'});
    }
    managerchangpassword(pwd:string)
    {
        this.authorization();
        return this.http.put(`http://localhost:5077/api/manager/Change-Password-Manager?NewPassword=${pwd}`,null,{headers:this.headers, responseType:'text'});
    }
}