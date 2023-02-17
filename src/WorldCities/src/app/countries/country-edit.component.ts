import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { map, Observable } from 'rxjs';
import { BaseFormComponent } from '../base-form.component';
import { Country } from './country';
import { CountryService } from './country.service';

@Component({
  selector: 'app-country-edit',
  templateUrl: './country-edit.component.html',
  styleUrls: ['./country-edit.component.scss']
})
export class CountryEditComponent extends BaseFormComponent implements OnInit {
  country!: Country;
  id?: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private countryService: CountryService) {
    super();
    this.form = new FormBuilder().group({
      name: ['', Validators.required, this.isDuplicatedField("name")],
      iso2: ['', [Validators.required, Validators.pattern(/^[A-Z]{2}$/)], this.isDuplicatedField("iso2")],
      iso3: ['', [Validators.required, Validators.pattern(/^[A-Z]{3}$/)], this.isDuplicatedField("iso3")]
    });
  }

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
      return this.countryService.isDuplicatedField(fieldName, control.value)
        .pipe(map(result => {
          return result.data.length > 0 && !this.country ? { isDuplicatedField: true } : null;
        }));
    }
    return isDuplicatedFieldValidator;
  }

  getCountry(id: number): void {
    this.countryService.get(id).subscribe({
      next: country => {
        this.country = country;
        this.form.patchValue(this.country);
      },
      error: err => console.error(err)
    });
  }

  createCountry(country: Country): void {
    this.countryService.post(country).subscribe({
      next: () => console.log(`Country ${country.name} has been created.`),
      error: err => console.error(err)
    })
  }

  updateCountry(country: Country): void {
    this.countryService.put(country).subscribe({
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
      country.countryId = this.country.countryId
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
