import { Injectable } from '@angular/core';
import { ApiService } from './baseApiService/api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class SchoolService extends ApiService {

  constructor(http: HttpClient) {
    super(http);
  }

  public GetStudentsByClassId(id: number): Observable<any> {
    return this.sendRequest('GET', 'Group', id);
  }
  public GetClasses(): Observable<any> {
    return this.sendRequest('GET', 'Groups');
  }
}
