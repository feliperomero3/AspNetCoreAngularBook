import { HttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { HealthCheckComponent, Result } from './health-check.component';

describe('HealthCheckComponent', () => {
  let component: HealthCheckComponent;
  let fixture: ComponentFixture<HealthCheckComponent>;
  let httpClientSpy: jasmine.SpyObj<HttpClient>;
  let healthChecks: Result[];

  beforeEach(async () => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    await TestBed.configureTestingModule({
      declarations: [HealthCheckComponent],
      providers: [
        { provide: HttpClient, useValue: httpClientSpy }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HealthCheckComponent);
    component = fixture.componentInstance;
    httpClientSpy.get.and.returnValue(of(healthChecks));
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });
});
