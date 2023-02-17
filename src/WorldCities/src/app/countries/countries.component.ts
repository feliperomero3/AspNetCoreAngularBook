import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { Country } from './country';
import { CountryService } from './country.service';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss']
})
export class CountriesComponent implements OnInit, AfterViewInit {
  displayedColumns = ['countryId', 'name', 'iso2', 'iso3', 'citiesCount'];
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

  filterTextChanged: Subject<string> = new Subject<string>();

  constructor(private countryService: CountryService) {
    this.pageEvent = new PageEvent();
    this.pageEvent.pageIndex = this.defaultPageIndex;
    this.pageEvent.pageSize = this.defaultPageSize;
  }

  ngAfterViewInit(): void {
    if (this.countries) {
      this.countries.paginator = this.paginator;
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
    this.getCountries(this.pageEvent);
  }

  getCountries(pageEvent: PageEvent): void {
    this.countryService.getData(
      pageEvent.pageIndex,
      pageEvent.pageSize,
      (this.sort ? this.sort.active : this.defaultSortColumn),
      (this.sort ? this.sort.direction : this.defaultSortOrder),
      this.defaultFilterColumn,
      this.filterQuery)
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
