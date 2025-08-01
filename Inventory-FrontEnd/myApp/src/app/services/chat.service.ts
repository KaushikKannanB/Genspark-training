
import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http"
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatbotModalService {
  private _isVisible = new BehaviorSubject<boolean>(false);
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
  // Observable for chatbot visibility
  visibility$ = this._isVisible.asObservable();

  open() {
    this._isVisible.next(true);
  }

  close() {
    this._isVisible.next(false);
  }

  answers(question: string) {
  this.authorization();
  return this.http.get(`http://localhost:5077/api/product/ask-specific-questions?question=${question}`, {
    headers: this.headers,
    responseType: 'text' as 'text'
  });
}
}