import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss']
})
export class CityEditComponent implements OnInit {
  city!: City;
  form = new FormGroup({
    name: new FormControl(''),
    latitude: new FormControl(''),
    longitude: new FormControl('')
  });

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    this.http.get<City>(environment.baseUrl + `api/cities/${id}`).subscribe({
      next: city => {
        this.city = city;
        this.form.patchValue(this.city);
      },
      error: err => console.error(err)
    });
  }

  onSubmit(): void {
    let city = this.city;
    city.name = this.form.controls['name'].value;
    city.latitude = this.form.controls['latitude'].value;
    city.longitude = this.form.controls['longitude'].value;

    this.http.put<City>(environment.baseUrl + `api/cities/${city.cityId}`, city).subscribe({
      next: () => {
        console.log(`City ${city.name} has been updated.`)
        this.router.navigate(['/cities']);
      },
      error: err => console.error(err)
    })
  }
}
