import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss']
})
export class CitiesComponent implements OnInit, AfterViewInit {
  displayedColumns = ['cityId', 'name', 'latitude', 'longitude'];
  cities!: MatTableDataSource<City>;
  pageEvent: PageEvent;
  defaultPageIndex = 0;
  defaultPageSize = 10;
  defaultSortColumn = 'name';
  defaultSortOrder: "asc" | "desc" = "asc";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private http: HttpClient) {
    this.pageEvent = new PageEvent();
  }

  ngAfterViewInit(): void {
    this.cities.paginator = this.paginator;
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.pageEvent.pageIndex = 0;
    this.pageEvent.pageSize = 10;
    this.getCities(this.pageEvent);
  }

  getCities(pageEvent: PageEvent): void {
    let params = new HttpParams()
      .set("pageIndex", pageEvent.pageIndex.toString())
      .set("pageSize", pageEvent.pageSize.toString())
      .set("sortColumn", this.sort ? this.sort.active : this.defaultSortColumn)
      .set("sortOrder", this.sort ? this.sort.direction : this.defaultSortOrder);

    this.http.get<any>(environment.baseUrl + 'api/cities', { params: params })
      .subscribe({
        next: result => {
          this.paginator.length = result.totalCount;
          this.paginator.pageIndex = result.pageIndex;
          this.paginator.pageSize = result.pageSize;
          this.cities = new MatTableDataSource<City>(result.data);
        },
        error: err => console.error(err)
      });
  }
}
