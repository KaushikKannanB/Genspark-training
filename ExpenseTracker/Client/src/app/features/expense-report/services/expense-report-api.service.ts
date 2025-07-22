import { Injectable } from '@angular/core';
import { HttpService } from '@core/services/http.service';
import { saveAs } from 'file-saver';
import { HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ExpenseReportApiService {
  constructor(private http: HttpService) {}

  private buildHttpParams(params: any): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(params || {}).forEach(key => {
      if (params[key] !== undefined && params[key] !== null) {
        httpParams = httpParams.set(key, params[key].toString());
      }
    });
    return httpParams;
  }

  downloadCSV(params: any, filename: string): void {
    const httpParams = this.buildHttpParams(params);

    this.http.downloadBlob(`/Expense/export/csv`, httpParams)
      .subscribe(blob => {
        saveAs(blob, filename);
      });
  }
  sendCSVEmail(params: any, email: string, filename: string): void {
    const httpParams = this.buildHttpParams(params);

    this.http.downloadBlob(`/Expense/export/csv`, httpParams).subscribe(blob => {
      const file = new File([blob], filename, { type: blob.type });

      const formData = new FormData();
      formData.append('Email', email);
      formData.append('File', file);

      this.http.post(`/User/send`, formData).subscribe({
        next: () => alert('Email sent successfully!'),
        error: err => {
          console.log(err);
          alert('Failed to send email: ' + (err?.error || err?.message))}
      });
    });
  }

}
