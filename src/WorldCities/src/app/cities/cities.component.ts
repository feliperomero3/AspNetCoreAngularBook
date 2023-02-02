import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss']
})
export class CitiesComponent implements OnInit {
  public displayedColumns: string[] = ['cityId', 'name', 'latitude', 'longitude'];
  public cities: City[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getCities();
  }

  getCities(): void {
    this.http.get<City[]>(environment.baseUrl + 'api/cities')
      .subscribe({
        next: (cities) => this.cities = cities,
        error: (err) => console.error(err)
      });
  }
}
