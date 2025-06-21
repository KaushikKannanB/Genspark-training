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
        console.log(update);
        return this.http.put('http://localhost:5077/api/product/Stock-Update',update, {headers:this.headers});
    }
}