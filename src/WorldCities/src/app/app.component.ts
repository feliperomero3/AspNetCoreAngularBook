import { Component, OnInit } from '@angular/core';
import { ConnectionService } from 'angular-connection-service';
import { AuthenticationService } from './authentication/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  hasNetworkConnection = false;
  hasInternetAccess = false;

  constructor(
    private authentication: AuthenticationService,
    private connection: ConnectionService) {
  }

  get isOnline(): boolean {
    return this.hasNetworkConnection && this.hasInternetAccess;
  }

  ngOnInit(): void {
    this.authentication.init();
    this.monitorConnection();
  }

  monitorConnection(): void {
    this.connection.monitor().subscribe({
      next: connectionState => {
        this.hasNetworkConnection = connectionState.hasNetworkConnection;
        this.hasInternetAccess = connectionState.hasInternetAccess;
      }
    });
  }
}
