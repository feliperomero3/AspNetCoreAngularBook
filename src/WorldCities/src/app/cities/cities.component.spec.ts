import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Directive, Input } from '@angular/core';

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
        StubDataSourceDirective
      ]
    }).compileComponents();

    httpTestingController = TestBed.inject(HttpTestingController);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the cities component', () => {
    expect(component).toBeTruthy();
  });

  it('should get the cities', () => {
    const testData: City[] = [{ cityId: 1, name: 'Tokyo', latitude: 100, longitude: 200 }];
    const req = httpTestingController.expectOne('api/cities');
    req.flush(testData);
    httpTestingController.verify();
    expect(component.cities.length).toBe(1);
  });
});
