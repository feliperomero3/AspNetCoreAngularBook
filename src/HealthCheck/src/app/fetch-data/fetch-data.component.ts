import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { environment } from '../../environments/environment';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.scss']
})
export class FetchDataComponent implements OnInit {
  public forecasts?: WeatherForecast[];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.getWeatherForecast();
  }

  getWeatherForecast(): void {
    this.http.get<WeatherForecast[]>(`${environment.baseUrl}/weatherforecast`).subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
