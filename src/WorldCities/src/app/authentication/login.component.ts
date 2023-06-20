import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from '../base-form.component';
import { AuthenticationService } from './authentication.service';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';
import { XsrfService } from './xsrf.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends BaseFormComponent implements OnInit {
  loginResult?: LoginResult;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private xsrf: XsrfService) {
    super();
    this.form = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    var loginRequest = <LoginRequest>{
      email: this.form.controls['email'].value,
      password: this.form.controls['password'].value
    };
    this.authService.login(loginRequest).subscribe({
      next: result => {
        console.log(JSON.stringify(result));
        this.loginResult = result;
        if (result.success) {
          console.log('Successful login');
          this.getXsrfToken();
          const returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl');
          if (returnUrl) {
            console.log(`returnUrl: ${returnUrl}`);
            this.router.navigate([`${returnUrl}`]);
          } else {
            this.router.navigate(['/']);
          }
        }
      },
      error: error => {
        console.log(error);
        if (error.status == 401) {
          this.loginResult = error.error;
        }
      }
    });
  }

  getXsrfToken(): void {
    this.xsrf.getXsrfToken().subscribe();
  }
}
