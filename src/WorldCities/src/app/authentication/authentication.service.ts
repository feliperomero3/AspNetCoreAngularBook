import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public cookieName = '.AspNetCore.Identity.Application';

  constructor(protected http: HttpClient) { }

  get isAuthenticated(): boolean {
    return this.getCookie() !== null;
  }

  private getCookie(): string | null {
    return getCookie(this.cookieName);
  }

  login(item: LoginRequest): Observable<LoginResult> {
    const url = environment.baseUrl + "api/account/login";
    const result: Observable<LoginResult> = this.http.post<LoginResult>(url, item, { observe: 'response' })
      .pipe(map(response => {
        const keys = response.headers.keys();
        const headers = keys.map(key => `${key}: ${response.headers.get(key)}`);
        console.log(JSON.stringify(headers));
        return { ...response.body! };
      }));
    return result;
  }

  logout(): Observable<{}> {
    var url = environment.baseUrl + "api/account/logout";
    return this.http.post(url, null);
  }
}

// https://developer.mozilla.org/en-US/docs/web/api/document/cookie#example_2_get_a_sample_cookie_named_test2
function getCookie(name: string): string | null {
  const cookieValue = document.cookie.split("; ").find((row) => row.startsWith(`"${name}="`))?.split("=")[1];
  return cookieValue ?? null;
}
