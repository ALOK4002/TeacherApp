import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Subscription, UploadEligibility } from '../models/subscription.models';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {
  private apiUrl = `${environment.apiUrl}/subscription`;

  constructor(private http: HttpClient) {}

  getMySubscription(): Observable<Subscription> {
    return this.http.get<Subscription>(`${this.apiUrl}/my-subscription`);
  }

  canUploadDocument(fileSizeInBytes: number): Observable<UploadEligibility> {
    return this.http.get<UploadEligibility>(`${this.apiUrl}/can-upload?fileSizeInBytes=${fileSizeInBytes}`);
  }

  incrementDocumentCount(): Observable<any> {
    return this.http.post(`${this.apiUrl}/increment-document-count`, {});
  }
}