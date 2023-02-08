import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Component, Directive, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { CountriesComponent } from './countries.component';
import { Country } from './country';

@Directive({
  selector: '[matRowDef]'
})
class StubMatRowDefDirective {
  @Input() matRowDefColumns: string[] = [];
}
@Directive({
  selector: '[matHeaderRowDef]'
})
class StubMatHeaderRowDefDirective {
  @Input() matHeaderRowDef: string = '';
}
@Directive({
  selector: '[dataSource]'
})
class StubDataSourceDirective {
  @Input() dataSource: string = '';
}
@Component({
  selector: 'mat-paginator'
})
class StubMatPaginatorComponent {
  @Input() pageSize?: number;
  @Input() pageSizeOptions?: number[];
}
@Component({
  selector: 'mat-form-field'
})
class StubMatFormFieldComponent { }
@Component({
  selector: 'mat-form-label'
})
class StubMatLabelComponent { }

describe('CountriesComponent', () => {
  let component: CountriesComponent;
  let fixture: ComponentFixture<CountriesComponent>;
  let httpTestingController: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [
        CountriesComponent,
        StubMatRowDefDirective,
        StubMatHeaderRowDefDirective,
        StubDataSourceDirective,
        StubMatPaginatorComponent,
        StubMatFormFieldComponent,
        StubMatLabelComponent
      ]
    }).compileComponents();

    httpTestingController = TestBed.inject(HttpTestingController);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CountriesComponent);
    component = fixture.componentInstance;
    component.countries = new MatTableDataSource<Country>();
    fixture.detectChanges();
  });

  it('should create the countries component', () => {
    expect(component).toBeTruthy();
  });
});
