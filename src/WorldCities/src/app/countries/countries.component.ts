import { HttpClient, HttpParams } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Country } from './country';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss']
})
export class CountriesComponent implements OnInit, AfterViewInit {
  displayedColumns = ['countryId', 'name', 'iso2', 'iso3'];
  countries!: MatTableDataSource<Country>;
  pageEvent: PageEvent;
  defaultPageIndex = 0;
  defaultPageSize = 10;
  defaultSortColumn = 'name';
  defaultSortOrder: "asc" | "desc" = "asc";
  defaultFilterColumn = "name";
  filterQuery?: string;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private http: HttpClient) {
    this.pageEvent = new PageEvent();
    this.pageEvent.pageIndex = this.defaultPageIndex;
    this.pageEvent.pageSize = this.defaultPageSize;
  }

  ngAfterViewInit(): void {
    this.countries.paginator = this.paginator;
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(query?: string): void {
    this.filterQuery = query;
    this.getCountries(this.pageEvent);
  }

  getCountries(pageEvent: PageEvent): void {
    let params = new HttpParams()
      .set("pageIndex", pageEvent.pageIndex.toString())
      .set("pageSize", pageEvent.pageSize.toString())
      .set("sortColumn", this.sort ? this.sort.active : this.defaultSortColumn)
      .set("sortOrder", this.sort ? this.sort.direction : this.defaultSortOrder);
    if (this.filterQuery) {
      params = params
        .set("filterColumn", this.defaultFilterColumn)
        .set("filterQuery", this.filterQuery);
    }
    this.http.get<any>(environment.baseUrl + 'api/countries', { params: params })
      .subscribe({
        next: result => {
          if (this.paginator) {
            this.paginator.length = result.totalCount;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
          }
          this.countries = new MatTableDataSource<Country>(result.data);
        },
        error: err => console.error(err)
      });
  }
}
