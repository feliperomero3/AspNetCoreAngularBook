import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public tokenKey = 'token';

  constructor(protected http: HttpClient) { }

  get isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  login(item: LoginRequest): Observable<LoginResult> {
    var url = environment.baseUrl + "api/account/login";
    return this.http.post<LoginResult>(url, item);
  }

  logout(): Observable<{}> {
    var url = environment.baseUrl + "api/account/logout";
    return this.http.post(url, null);
  }
}
