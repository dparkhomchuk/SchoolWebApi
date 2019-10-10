import { Injectable } from '@angular/core';
import { extend } from 'webdriver-js-extender';
import { ApiService } from './baseApiService/api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentsService extends ApiService {

  constructor(http: HttpClient) {
    super(http);
  }
  public getStudentsByClassId(id: number): Observable<any> {
    return this.sendRequest('GET', 'group/'+ id);
  }
  public addStudent(student: any): Observable<any> {
    return this.sendRequest('POST', 'student', student);
  }
}
