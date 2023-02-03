import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss']
})
export class CitiesComponent implements OnInit, AfterViewInit {
  public displayedColumns: string[] = ['cityId', 'name', 'latitude', 'longitude'];
  public cities!: MatTableDataSource<City>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private http: HttpClient) { }

  ngAfterViewInit(): void {
    this.cities.paginator = this.paginator;
  }

  ngOnInit(): void {
    this.getCities();
  }

  getCities(): void {
    this.http.get<City[]>(environment.baseUrl + 'api/cities')
      .subscribe({
        next: (cities) => {
          this.cities = new MatTableDataSource<City>(cities);
        },
        error: (err) => console.error(err)
      });
  }
}
