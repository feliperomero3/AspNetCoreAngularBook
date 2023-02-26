import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private cookieName = '.AspNetCore.Identity.Application';
  private authenticationStatusSubject = new Subject<boolean>();

  authenticationStatus = this.authenticationStatusSubject.asObservable();

  constructor(protected http: HttpClient) { }

  get isAuthenticated(): boolean {
    return this.getCookie() !== null;
  }

  private getCookie(): string | null {
    return getCookie(this.cookieName);
  }

  private setAuthenticationStatus(isAuthenticated: boolean): void {
    this.authenticationStatusSubject.next(isAuthenticated);
  }

  init(): void {
    this.setAuthenticationStatus(this.isAuthenticated);
  }

  login(item: LoginRequest): Observable<LoginResult> {
    const url = environment.baseUrl + 'api/account/login';
    return this.http.post<LoginResult>(url, item)
      .pipe(map(response => {
        this.setAuthenticationStatus(true);
        return response;
      }));
  }

  logout(): Observable<{}> {
    var url = environment.baseUrl + 'api/account/logout';
    return this.http.post(url, null)
      .pipe(map(response => {
        this.setAuthenticationStatus(false)
        return response;
      }));
  }
}

function getCookie(name: string): string | null {
  const cookieValue = document.cookie.split("; ").find((row) => row.startsWith(`${name}=`))?.split("=")[1];
  return cookieValue ?? null;
}
