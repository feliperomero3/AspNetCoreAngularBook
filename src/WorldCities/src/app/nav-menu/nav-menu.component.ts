import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthenticationService } from '../authentication/authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  private destroySubject = new Subject();
  isLoggedIn = false;

  constructor(private router: Router, private authentication: AuthenticationService) {
    this.isLoggedIn = this.authentication.isAuthenticated;
  }

  ngOnInit(): void {
    this.updateLoginStatus();
  }

  updateLoginStatus(): void {
    this.authentication.authenticationStatus
      .pipe(takeUntil(this.destroySubject))
      .subscribe(result => {
        this.isLoggedIn = result;
      })
  }

  onLogout(): void {
    this.authentication.logout();
    this.router.navigate(["/"]);
  }
}
