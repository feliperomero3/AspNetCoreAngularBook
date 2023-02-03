import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
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
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = 0;
    pageEvent.pageSize = 10;
    this.getCities(pageEvent);
  }

  getCities(pageEvent: PageEvent): void {
    var params = new HttpParams()
      .set("pageIndex", pageEvent.pageIndex.toString())
      .set("pageSize", pageEvent.pageSize.toString());

    this.http.get<any>(environment.baseUrl + 'api/cities', { params: params })
      .subscribe({
        next: (result) => {
          this.paginator.length = result.totalCount;
          this.paginator.pageIndex = result.pageIndex;
          this.paginator.pageSize = result.pageSize;
          this.cities = new MatTableDataSource<City>(result.data);
        },
        error: (err) => console.error(err)
      });
  }
}
