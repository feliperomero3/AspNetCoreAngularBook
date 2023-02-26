import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthenticationService } from '../authentication/authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  private destroySubject = new Subject();
  isLoggedIn = false;

  constructor(private router: Router, private authentication: AuthenticationService) {
  }

  ngOnDestroy(): void {
    this.destroySubject.next(true);
    this.destroySubject.complete();
  }

  ngOnInit(): void {
    this.getLoginStatus();
    this.updateLoginStatus();
  }

  getLoginStatus(): void {
    this.isLoggedIn = this.authentication.isAuthenticated;
  }

  updateLoginStatus(): void {
    this.authentication.authenticationStatus
      .pipe(takeUntil(this.destroySubject))
      .subscribe(result => {
        this.isLoggedIn = result;
      })
  }

  onLogout(): void {
    this.authentication.logout().subscribe({
      next: () => this.router.navigate(["/"]),
      error: error => console.log(error)
    });
  }
}
