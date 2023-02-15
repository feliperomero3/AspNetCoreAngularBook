import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { environment } from './../../environments/environment';
import { City } from './city';
import { Country } from '../countries/country';
import { BaseFormComponent } from '../base-form.component';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss']
})
export class CityEditComponent extends BaseFormComponent implements OnInit {
  city!: City;
  id?: number;
  countries?: Country[];

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient) {
    super();
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      latitude: new FormControl('', [Validators.required, Validators.pattern(/^[-]?[0-9]+(\.[0-9]{1,4})?$/)]),
      longitude: new FormControl('', [Validators.required, Validators.pattern(/^[-]?[0-9]+(\.[0-9]{1,4})?$/)]),
      countryId: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {
    this.getCountries();
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getCity(+id);
    }
  }

  getCountries(): void {
    var params = new HttpParams()
      .set("pageIndex", "0")
      .set("pageSize", "9999")
      .set("sortColumn", "name");
    this.http.get<any>(environment.baseUrl + 'api/countries', { params: params }).subscribe({
      next: result => {
        this.countries = result.data;
      },
      error: err => console.error(err)
    });
  }

  getCity(id: number): void {
    this.http.get<City>(environment.baseUrl + `api/cities/${id}`).subscribe({
      next: city => {
        this.city = city;
        this.form.patchValue(this.city);
      },
      error: err => console.error(err)
    });
  }

  createCity(city: City): void {
    this.http.post<City>(environment.baseUrl + `api/cities`, city).subscribe({
      next: () => {
        console.log(`City ${city.name} has been created.`)
      },
      error: err => console.error(err)
    })
  }

  updateCity(city: City): void {
    this.http.put<City>(environment.baseUrl + `api/cities/${city.cityId}`, city).subscribe({
      next: () => {
        console.log(`City ${city.name} has been updated.`)
      },
      error: err => console.error(err)
    })
  }

  onSubmit(): void {
    let city = <City>{};
    city.name = this.form.controls['name'].value;
    city.latitude = +this.form.controls['latitude'].value;
    city.longitude = +this.form.controls['longitude'].value;
    city.countryId = +this.form.controls['countryId'].value;
    if (this.city) {
      city.cityId = this.city.cityId;
      this.updateCity(city);
    } else {
      this.createCity(city);
    }
    this.router.navigate(['/cities']);
  }

  onCancel(): void {
    this.router.navigate(['/cities']);
  }
}
