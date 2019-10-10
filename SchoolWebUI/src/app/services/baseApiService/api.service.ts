import { Injectable } from '@angular/core';
import { AppConfig } from '../../config';
import { map, retry, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService {
  protected baseUrl = environment.apiBaseUrl;

protected constructor(private http: HttpClient) { }

protected sendRequest<T>(verb: string, url: string, body?): Observable<T> {

  return this.http.request<T>(verb, this.baseUrl + url, {
    body
  }).pipe(
    tap(),
    retry(3)
  );
}

}
