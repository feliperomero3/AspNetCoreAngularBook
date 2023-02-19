import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Component, Directive, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { CitiesComponent } from './cities.component';
import { City } from './city';
import { CityService } from './city.service';
import { of } from 'rxjs';
import { ApiResult } from '../base.service';

@Directive({
  selector: '[matRowDef]'
})
export class StubMatRowDefDirective {
  @Input() matRowDefColumns: string[] = [];
}
@Directive({
  selector: '[matHeaderRowDef]'
})
export class StubMatHeaderRowDefDirective {
  @Input() matHeaderRowDef: string = '';
}
@Directive({
  selector: '[dataSource]'
})
export class StubDataSourceDirective {
  @Input() dataSource: string = '';
}
@Component({
  selector: 'mat-paginator'
})
class StubMatPaginatorComponent {
  @Input() pageSize?: number;
  @Input() pageSizeOptions?: number[];
}

describe('CitiesComponent', () => {
  let component: CitiesComponent;
  let fixture: ComponentFixture<CitiesComponent>;

  let testData = <ApiResult<City>>{
    data: [
      {
        cityId: 1,
        name: 'Tokyo',
        latitude: 35.6839,
        longitude: 139.7744,
        countryId: 1,
        countryName: 'Japan'
      },
      {
        cityId: 2,
        name: 'Jakarta',
        latitude: -6.2146,
        longitude: 106.8451,
        countryId: 2,
        countryName: 'Indonesia'
      },
      {
        cityId: 3,
        name: 'Delhi',
        latitude: 28.6667,
        longitude: 77.2167,
        countryId: 3,
        countryName: 'India'
      }
    ],
    totalCount: 3,
    pageIndex: 0,
    pageSize: 10
  };

  let mockCityService = jasmine.createSpyObj<CityService>('CityService', ['getData']);

  mockCityService.getData.and.returnValue(of(testData));

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [
        CitiesComponent,
        StubMatRowDefDirective,
        StubMatHeaderRowDefDirective,
        StubDataSourceDirective,
        StubMatPaginatorComponent
      ],
      providers: [
        { provide: CityService, useValue: mockCityService }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitiesComponent);
    component = fixture.componentInstance;
    component.cities = new MatTableDataSource<City>([]);
    component.paginator = jasmine.createSpyObj("MatPaginator", ["length", "pageIndex", "pageSize"]);
    fixture.detectChanges();
  });

  it('should create the cities component', () => {
    expect(component).toBeTruthy();
  });

  it('should get the cities', () => {
    expect(component.cities.data.length).toBe(testData.data.length);
  });
});
