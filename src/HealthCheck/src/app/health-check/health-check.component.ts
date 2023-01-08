import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { environment } from '../../environments/environment';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrls: ['./health-check.component.scss']
})
export class HealthCheckComponent implements OnInit {
  public result?: Result | undefined;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getHealthCheck();
  }

  getHealthCheck(): void {
    this.http.get<Result>(`${environment.baseUrl}/health`).subscribe(result => {
      this.result = result;
    }, error => console.error(error));
  }
}

export interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}

interface Check {
  name: string;
  responseTime: number;
  status: string;
  description: string;
}
