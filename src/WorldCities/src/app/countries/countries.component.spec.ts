import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableDataSource } from '@angular/material/table';
import { AngularMaterialModule } from '../angular-material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CountriesComponent } from './countries.component';
import { Country } from './country';

describe('CountriesComponent', () => {
  let component: CountriesComponent;
  let fixture: ComponentFixture<CountriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        BrowserAnimationsModule,
        AngularMaterialModule
      ],
      declarations: [
        CountriesComponent
      ]
    }).compileComponents();
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
