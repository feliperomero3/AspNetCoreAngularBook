import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { CitiesComponent } from './cities.component';
import { City } from './city';

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
        CitiesComponent
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
