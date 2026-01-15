import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { School, CreateSchool, UpdateSchool } from '../models/school.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SchoolService {
  private apiUrl = `${environment.apiUrl}/school`;

  constructor(private http: HttpClient) {}

  getAllSchools(): Observable<School[]> {
    return this.http.get<School[]>(this.apiUrl);
  }

  getSchoolById(id: number): Observable<School> {
    return this.http.get<School>(`${this.apiUrl}/${id}`);
  }

  createSchool(school: CreateSchool): Observable<School> {
    return this.http.post<School>(this.apiUrl, school);
  }

  updateSchool(id: number, school: UpdateSchool): Observable<School> {
    return this.http.put<School>(`${this.apiUrl}/${id}`, school);
  }

  deleteSchool(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getSchoolsByDistrict(district: string): Observable<School[]> {
    return this.http.get<School[]>(`${this.apiUrl}/district/${district}`);
  }
}