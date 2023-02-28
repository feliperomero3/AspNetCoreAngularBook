import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class XsrfService {

  constructor(private http: HttpClient) { }

  getXsrfToken(): Observable<{}> {
    return this.http.get('api/xsrf-token')
      .pipe(response => {
        console.log('xsrf called');
        return response;
      });
  }
}
