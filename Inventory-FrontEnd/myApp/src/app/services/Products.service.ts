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
        return this.http.get('http://localhost:5077/api/product/Get-All-Products', {headers:this.headers });
    }
    getstocksbyproductname(product:string)
    {
        this.authorization();
        return this.http.get(`http://localhost:5077/api/product/Get-Stock-By-ProductName?product=${product}`,{headers:this.headers});
    }
    addproduct(product:ProductAddModel)
    {
        this.authorization();
        console.log(product);
        return this.http.post('http://localhost:5077/api/product/Add-Product',product,{headers:this.headers})
    }
    deleteproduct(productname:string)
    {
        this.authorization();
        return this.http.delete(`http://localhost:5077/api/product/Delete-Product?product=${productname}`,{headers:this.headers});
    }
    updatestock(update:StockUpdateModel)
    {
        this.authorization();
        return this.http.put('http://localhost:5077/api/product/Stock-Update',update, {headers:this.headers});
    }
    getallcategories(){
        this.authorization();
        return this.http.get('http://localhost:5077/api/product/Get-All-Categories', {headers:this.headers});
    }
    getcategorybyId(id:string){
        this.authorization();
        return this.http.get(`http://localhost:5077/api/product/Get-category-by-Id?id=${id}`, {headers:this.headers});
    }
    getallstocklogs()
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/product/Get-All-Stock-updates',{headers:this.headers});
    }
    getprodbyinv(inv:string)
    {
        this.authorization();
        return this.http.get(`http://localhost:5077/api/product/Get-product-by-inventory-id?id=${inv}`,{headers:this.headers});
    }
    getprodnamebyid(id:string)
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/product/Get-Productby-Id?id='+id,{headers:this.headers});
    }
    getallproductlogs()
    {
        this.authorization();
        return this.http.get('http://localhost:5077/api/product/Get-All-Product-Updates',{headers:this.headers});
    }
    updateprodprice(payload:any)
    {
        this.authorization();
        return this.http.put('http://localhost:5077/api/product/Update-Product-price',payload, {headers:this.headers})
    }
    updateproddescription(payload:any)
    {
        this.authorization();
        return this.http.put('http://localhost:5077/api/product/Update-Product-description',payload,{headers:this.headers});
    }
    
}