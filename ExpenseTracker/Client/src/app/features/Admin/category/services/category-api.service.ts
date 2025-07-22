// services/category-api.service.ts
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HttpService } from '@core/services/http.service';
import { Category } from '@shared/models/category.model';
import { ApiResponse } from '@shared/models/category.model';
import { RegisterRequest } from '@shared/models/auth.model';
import { HttpClient } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class CategoryApiService {
  private readonly endpoint = '/Category';
  private readonly endpoint_analyser = '/User/create-analyser';
  private readonly endpoint_creds_send = '/User/send-analyer-credentials';

  constructor(private httpService: HttpService) {}

  getAllCategories(): Observable<Category[]> {
    return this.httpService
      .get<ApiResponse<Category[]>>(this.endpoint)
      .pipe(map((res) => res.data));
  }

  getCategoryById(id: string): Observable<Category> {
    return this.httpService
      .get<ApiResponse<Category>>(`${this.endpoint}/${id}`)
      .pipe(map((res) => res.data));
  }

  createCategory(category: { name: string }): Observable<Category> {
  return this.httpService
    .post<ApiResponse<Category>>(this.endpoint, category)
    .pipe(map((res) => res.data));
}


  updateCategory(id: string, category: Category): Observable<Category> {
    return this.httpService
      .put<ApiResponse<Category>>(`${this.endpoint}/${id}`, category)
      .pipe(map((res) => res.data));
  }

  deleteCategory(id: string): Observable<void> {
    return this.httpService
      .delete<ApiResponse<null>>(`${this.endpoint}/${id}`)
      .pipe(map(() => void 0)); 
  }

  addAnalyer(req: RegisterRequest)
  {
    return this.httpService.post(this.endpoint_analyser,req);
  }
  credentials_email(req: RegisterRequest) {
    return this.httpService.post(`${this.endpoint_creds_send}?email=${req.email}&content=${req.password}`, null);
  }
}
