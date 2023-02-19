import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { CityService } from './city.service';

describe('CityService', () => {
  let service: CityService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(CityService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the cities', () => {
    service.getData(0, 10, 'name', 'asc', null, null).subscribe();
    httpTestingController.expectOne('api/cities?pageIndex=0&pageSize=10&sortColumn=name&sortOrder=asc');
    httpTestingController.verify();
  });
});
