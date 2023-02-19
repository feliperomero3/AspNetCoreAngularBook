import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { City } from './city';
import { CityService } from './city.service';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss']
})
export class CitiesComponent implements OnInit, AfterViewInit {
  displayedColumns = ['cityId', 'name', 'latitude', 'longitude', 'countryName'];
  cities!: MatTableDataSource<City>;
  pageEvent: PageEvent;
  defaultPageIndex = 0;
  defaultPageSize = 10;
  defaultSortColumn = 'name';
  defaultSortOrder: "asc" | "desc" = "asc";
  defaultFilterColumn = "name";
  filterQuery?: string;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  filterTextChanged: Subject<string> = new Subject<string>();

  constructor(private cityService: CityService, private http: HttpClient) {
    this.pageEvent = new PageEvent();
    this.pageEvent.pageIndex = this.defaultPageIndex;
    this.pageEvent.pageSize = this.defaultPageSize;
  }

  ngAfterViewInit(): void {
    if (this.cities) {
      this.cities.paginator = this.paginator;
    }
  }

  ngOnInit(): void {
    this.loadData();
  }

  onFilterTextChanged(filterText: string): void {
    if (!this.filterTextChanged.observed) {
      this.filterTextChanged
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe(text => this.loadData(text));
    }
    this.filterTextChanged.next(filterText);
  }

  loadData(query?: string): void {
    this.filterQuery = query;
    this.getCities(this.pageEvent);
  }

  getCities(pageEvent: PageEvent): void {
    this.cityService.getData(
      pageEvent.pageIndex,
      pageEvent.pageSize,
      (this.sort ? this.sort.active : this.defaultSortColumn),
      (this.sort ? this.sort.direction : this.defaultSortOrder),
      this.defaultFilterColumn,
      this.filterQuery)
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
