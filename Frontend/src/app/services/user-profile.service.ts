import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserProfile, CreateUserProfile, UpdateUserProfile } from '../models/user-profile.models';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private apiUrl = `${environment.apiUrl}/userprofile`;

  constructor(private http: HttpClient) {}

  getMyProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/my-profile`);
  }

  hasProfile(): Observable<boolean> {
    return new Observable<boolean>(observer => {
      this.http.get<{ hasProfile: boolean }>(`${this.apiUrl}/has-profile`).subscribe({
        next: (response) => {
          observer.next(response.hasProfile);
          observer.complete();
        },
        error: (err) => observer.error(err)
      });
    });
  }

  createProfile(profile: CreateUserProfile): Observable<UserProfile> {
    return this.http.post<UserProfile>(`${this.apiUrl}`, profile);
  }

  updateProfile(id: number, profile: UpdateUserProfile): Observable<UserProfile> {
    return this.http.put<UserProfile>(`${this.apiUrl}/${id}`, profile);
  }
}
