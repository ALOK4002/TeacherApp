import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { Notice, CreateNotice, UpdateNotice, CreateNoticeReply, NoticeWithReplies } from '../models/notice.models';

@Injectable({
  providedIn: 'root'
})
export class NoticeService {
  private apiUrl = 'http://localhost:5162/api/notice';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

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