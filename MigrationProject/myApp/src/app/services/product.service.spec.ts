import { TestBed } from '@angular/core/testing';
import { ProductService } from './Products.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductAddModel } from '../models/product';
import { StockUpdateModel } from '../models/StockUpdateModel';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  const token = 'mocked-token';

  beforeEach(() => {
    localStorage.setItem('token', token);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.removeItem('token');
  });

  const expectAuthHeader = (req: any) => {
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
  };

  it('should fetch all products', () => {
    service.getallproducts().subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-All-Products');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should fetch stocks by product name', () => {
    service.getstocksbyproductname('Shampoo').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-Stock-By-ProductName?product=Shampoo');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should add a product', () => {
    const product: ProductAddModel = { name: 'Soap', price: 25};
    service.addproduct(product).subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Add-Product');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(product);
    expectAuthHeader(req);
    req.flush({});
  });

  it('should delete a product', () => {
    service.deleteproduct('Soap').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Delete-Product?product=Soap');
    expect(req.request.method).toBe('DELETE');
    expectAuthHeader(req);
    req.flush({});
  });

  it('should update stock', () => {
    const stockUpdate: StockUpdateModel = { ProductName:'cococola' };
    service.updatestock(stockUpdate).subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Stock-Update');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(stockUpdate);
    expectAuthHeader(req);
    req.flush({});
  });

  it('should get all categories', () => {
    service.getallcategories().subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-All-Categories');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should get category by id', () => {
    service.getcategorybyId('2').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-category-by-Id?id=2');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush({});
  });

  it('should get all stock logs', () => {
    service.getallstocklogs().subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-All-Stock-updates');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should get product by inventory id', () => {
    service.getprodbyinv('inv123').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-product-by-inventory-id?id=inv123');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush({});
  });

  it('should get product name by id', () => {
    service.getprodnamebyid('p123').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-Productby-Id?id=p123');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush({});
  });

  it('should get all product logs', () => {
    service.getallproductlogs().subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-All-Product-Updates');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should update product price', () => {
    const payload = { id: '1', price: 99 };
    service.updateprodprice(payload).subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Update-Product-price');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(payload);
    expectAuthHeader(req);
    req.flush({});
  });

  it('should update product description', () => {
    const payload = { id: '1', description: 'New desc' };
    service.updateproddescription(payload).subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Update-Product-description');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(payload);
    expectAuthHeader(req);
    req.flush({});
  });

  it('should send category add request', () => {
    service.makecategoryaddrequest('NewCat').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/manager/Category-Add-Requested?category=NewCat');
    expect(req.request.method).toBe('POST');
    expectAuthHeader(req);
    req.flush({});
  });

  it('should get stock logs by product name', () => {
    service.getstocklogsbyproductname('Soap').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-All-Stock-updates-For-ProductName?product=Soap');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush([]);
  });

  it('should get product summary', () => {
    service.getproducsummary('Soap').subscribe();
    const req = httpMock.expectOne('http://localhost:5077/api/product/Get-product-summary?prod=Soap');
    expect(req.request.method).toBe('GET');
    expectAuthHeader(req);
    req.flush({});
  });
});
