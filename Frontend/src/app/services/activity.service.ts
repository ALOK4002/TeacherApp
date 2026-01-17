import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserActivity } from '../models/activity.models';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private apiUrl = `${environment.apiUrl}/useractivity`;

  constructor(private http: HttpClient) {}

  getMyActivities(page: number = 1, pageSize: number = 20): Observable<UserActivity[]> {
    return this.http.get<UserActivity[]>(`${this.apiUrl}/my-activities?page=${page}&pageSize=${pageSize}`);
  }
}