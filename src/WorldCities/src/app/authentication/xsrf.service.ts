import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class XsrfService {

  constructor(private http: HttpClient) { }

  getXsrfToken(): Observable<{}> {
    const url = environment.baseUrl + 'api/xsrf-token';
    return this.http.get(url)
      .pipe(response => {
        console.log('xsrf called');
        return response;
      });
  }
}
