import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult, BaseService } from '../base.service';
import { Country } from './country';

@Injectable({
  providedIn: 'root'
})
export class CountryService extends BaseService<Country> {
  constructor(http: HttpClient) {
    super(http);
  }

  isDuplicatedField(fieldName: string, fieldValue: string): Observable<ApiResult<Country>> {
    var url = this.getUrl("api/countries");
    var params = new HttpParams()
      .set("filterColumn", fieldName)
      .set("filterQuery", fieldValue);
    return this.http.get<ApiResult<Country>>(url, { params });
  }

  getData(pageIndex: number, pageSize: number, sortColumn: string, sortOrder: string, filterColumn: string | null, filterQuery: string | null | undefined): Observable<ApiResult<Country>> {
    var url = this.getUrl("api/countries");
    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);
    if (filterColumn && filterQuery) {
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);
    }
    return this.http.get<ApiResult<Country>>(url, { params });
  }
  get(countryId: number): Observable<Country> {
    var url = this.getUrl("api/countries/" + countryId);
    return this.http.get<Country>(url);
  }
  put(country: Country): Observable<Country> {
    var url = this.getUrl("api/countries/" + country.countryId);
    return this.http.put<Country>(url, country);
  }
  post(country: Country): Observable<Country> {
    var url = this.getUrl("api/countries");
    return this.http.post<Country>(url, country);
  }
}
