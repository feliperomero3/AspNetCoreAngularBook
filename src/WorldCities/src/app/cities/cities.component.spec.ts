import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Component, Directive, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { CitiesComponent } from './cities.component';
import { City } from './city';

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
  let httpTestingController: HttpTestingController;

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
      ]
    }).compileComponents();

    httpTestingController = TestBed.inject(HttpTestingController);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitiesComponent);
    component = fixture.componentInstance;
    component.cities = new MatTableDataSource<City>([]);
    fixture.detectChanges();
  });

  it('should create the cities component', () => {
    expect(component).toBeTruthy();
  });

  it('should get the cities', () => {
    const testData = { data: [{ cityId: 1, name: 'Tokyo', latitude: 100, longitude: 200 }] };
    const req = httpTestingController.expectOne('api/cities?pageIndex=0&pageSize=10&sortColumn=name&sortOrder=asc');
    req.flush(testData);
    httpTestingController.verify();
    expect(component.cities.data.length).toBe(1);
  });
});
