import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { map, Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { Country } from './country';

@Component({
  selector: 'app-country-edit',
  templateUrl: './country-edit.component.html',
  styleUrls: ['./country-edit.component.scss']
})
export class CountryEditComponent implements OnInit {
  country!: Country;
  id?: number;
  form = new FormBuilder().group({
    name: ['', Validators.required, this.isDuplicatedField("name")],
    iso2: ['', Validators.required, Validators.pattern(/^[A-Z]{2}$/), this.isDuplicatedField("iso2")],
    iso3: ['', Validators.required, Validators.pattern(/^[A-Z]{3}$/), this.isDuplicatedField("iso3")]
  });

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient) { }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.getCountry(+id);
    }
  }

  isDuplicatedField(fieldName: string): AsyncValidatorFn {
    const isDuplicatedFieldValidator = (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      var params = new HttpParams()
        .set("filterColumn", fieldName)
        .set("filterQuery", control.value);
      return this.http.get<any>(environment.baseUrl + 'api/countries', { params })
        .pipe(map(result => {
          return result.data.length > 0 ? { isDuplicatedField: true } : null;
        }));
    }
    return isDuplicatedFieldValidator;
  }

  getCountry(id: number): void {
    this.http.get<Country>(environment.baseUrl + `api/countries/${id}`).subscribe({
      next: country => {
        this.country = country;
        this.form.patchValue(this.country);
      },
      error: err => console.error(err)
    });
  }

  createCountry(country: Country): void {
    this.http.post<Country>(environment.baseUrl + `api/countries`, country).subscribe({
      next: () => console.log(`Country ${country.name} has been created.`),
      error: err => console.error(err)
    })
  }

  updateCountry(country: Country): void {
    this.http.put<Country>(environment.baseUrl + `api/countries/${country.countryId}`, country).subscribe({
      next: () => console.log(`Country ${country.name} has been updated.`),
      error: err => console.error(err)
    })
  }

  onSubmit(): void {
    let country = <Country>{};
    country.name = this.form.controls['name'].value;
    country.iso2 = this.form.controls['iso2'].value;
    country.iso3 = this.form.controls['iso3'].value;
    if (this.country) {
      this.updateCountry(country);
    } else {
      this.createCountry(country);
    }
    this.router.navigate(['/countries']);
  }

  onCancel(): void {
    this.router.navigate(['/countries']);
  }
}