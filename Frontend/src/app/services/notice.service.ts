import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notice, CreateNotice, UpdateNotice, CreateNoticeReply, NoticeWithReplies } from '../models/notice.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NoticeService {
  private apiUrl = `${environment.apiUrl}/notice`;

  constructor(private http: HttpClient) {}

  getAllNotices(): Observable<Notice[]> {
    return this.http.get<Notice[]>(this.apiUrl);
  }

  getNoticeWithReplies(id: number): Observable<NoticeWithReplies> {
    return this.http.get<NoticeWithReplies>(`${this.apiUrl}/${id}`);
  }

  getMyNotices(): Observable<Notice[]> {
    return this.http.get<Notice[]>(`${this.apiUrl}/my-notices`);
  }

  createNotice(notice: CreateNotice): Observable<Notice> {
    return this.http.post<Notice>(this.apiUrl, notice);
  }

  updateNotice(id: number, notice: UpdateNotice): Observable<Notice> {
    return this.http.put<Notice>(`${this.apiUrl}/${id}`, notice);
  }

  deleteNotice(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  addReply(reply: CreateNoticeReply): Observable<any> {
    return this.http.post(`${this.apiUrl}/reply`, reply);
  }
}