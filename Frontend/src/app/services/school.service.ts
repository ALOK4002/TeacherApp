import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { School, CreateSchool, UpdateSchool } from '../models/school.models';

@Injectable({
  providedIn: 'root'
})
export class SchoolService {
  private apiUrl = 'http://localhost:5162/api/school';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

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