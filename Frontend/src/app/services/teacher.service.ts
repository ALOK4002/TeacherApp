import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Teacher, CreateTeacher, UpdateTeacher, District, Pincode, TeacherReport, TeacherReportSearchRequest, PagedResult } from '../models/teacher.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl = `${environment.apiUrl}/teacher`;
  private utilityUrl = `${environment.apiUrl}/utility`;

  constructor(private http: HttpClient) {}

  getAllTeachers(): Observable<Teacher[]> {
    return this.http.get<Teacher[]>(this.apiUrl);
  }

  getTeacherById(id: number): Observable<Teacher> {
    return this.http.get<Teacher>(`${this.apiUrl}/${id}`);
  }

  createTeacher(teacher: CreateTeacher): Observable<Teacher> {
    return this.http.post<Teacher>(this.apiUrl, teacher);
  }

  updateTeacher(id: number, teacher: UpdateTeacher): Observable<Teacher> {
    return this.http.put<Teacher>(`${this.apiUrl}/${id}`, teacher);
  }

  deleteTeacher(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getTeachersByDistrict(district: string): Observable<Teacher[]> {
    return this.http.get<Teacher[]>(`${this.apiUrl}/district/${district}`);
  }

  getTeachersBySchool(schoolId: number): Observable<Teacher[]> {
    return this.http.get<Teacher[]>(`${this.apiUrl}/school/${schoolId}`);
  }

  // Utility methods
  getBiharDistricts(): Observable<District[]> {
    return this.http.get<District[]>(`${this.utilityUrl}/districts`);
  }

  getPincodesByDistrict(district: string): Observable<Pincode[]> {
    return this.http.get<Pincode[]>(`${this.utilityUrl}/pincodes/${district}`);
  }

  getDistrictByPincode(pincode: string): Observable<{district: string}> {
    return this.http.get<{district: string}>(`${this.utilityUrl}/district/${pincode}`);
  }

  getTeacherReport(request: TeacherReportSearchRequest): Observable<PagedResult<TeacherReport>> {
    return this.http.post<PagedResult<TeacherReport>>(`${this.apiUrl}/report`, request);
  }
}