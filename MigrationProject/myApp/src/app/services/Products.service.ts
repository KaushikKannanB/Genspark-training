import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { ProductAddModel } from "../models/product";
import { StockUpdateModel } from "../models/StockUpdateModel";

export class ProductService
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
    getallproducts()
    {
        this.authorization();
        return this.http.get('http://localhost:5048/api/products/get-all-unsold-products', {headers:this.headers });
    }
    
    addproduct(product:ProductAddModel)
    {
        this.authorization();
        console.log(product);
        return this.http.post('http://localhost:5077/api/product/Add-Product',product,{headers:this.headers})
    }
    
    
    getallcategories(){
        this.authorization();
        return this.http.get('http://localhost:5048/api/others/get-all-category', {headers:this.headers});
    }
    
    
    getprodnamebyid(id:string)
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/product/Get-Productby-Id?id='+id,{headers:this.headers});
    }
    
    
    
}