import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CitiesComponent } from './cities/cities.component';
import { CountriesComponent } from './countries/countries.component';
import { CityEditComponent } from './cities/city-edit.component';
import { CountryEditComponent } from './countries/country-edit.component';
import { LoginComponent } from './authentication/login.component';
import { AuthenticationGuard } from './authentication/authentication.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'cities', component: CitiesComponent, canActivate: [AuthenticationGuard] },
  { path: 'cities/:id', component: CityEditComponent, canActivate: [AuthenticationGuard] },
  { path: 'city', component: CityEditComponent, canActivate: [AuthenticationGuard] },
  { path: 'countries', component: CountriesComponent, canActivate: [AuthenticationGuard] },
  { path: 'country/:id', component: CountryEditComponent, canActivate: [AuthenticationGuard] },
  { path: 'country', component: CountryEditComponent, canActivate: [AuthenticationGuard] },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

