import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { Teacher, CreateTeacher, UpdateTeacher, District, Pincode } from '../models/teacher.models';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl = 'http://localhost:5162/api/teacher';
  private utilityUrl = 'http://localhost:5162/api/utility';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

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
}