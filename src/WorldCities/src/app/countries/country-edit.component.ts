import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
    name: ['', Validators.required],
    iso2: ['', Validators.required, Validators.pattern(/^[A-Z]{2}$/)],
    iso3: ['', Validators.required, Validators.pattern(/^[A-Z]{3}$/)]
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
