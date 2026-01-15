import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TeacherDocument, SendDocumentEmailRequest, DocumentSearchRequest, PagedResult } from '../models/document.models';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private apiUrl = `${environment.apiUrl}/teacherdocument`;

  constructor(private http: HttpClient) {}

  uploadDocument(teacherId: number, file: File, documentType: string, customDocumentType: string, remarks: string): Observable<TeacherDocument> {
    const formData = new FormData();
    formData.append('teacherId', teacherId.toString());
    formData.append('file', file);
    formData.append('documentType', documentType);
    formData.append('customDocumentType', customDocumentType || '');
    formData.append('remarks', remarks || '');

    return this.http.post<TeacherDocument>(`${this.apiUrl}/upload`, formData);
  }

  getDocumentsByTeacher(teacherId: number): Observable<TeacherDocument[]> {
    return this.http.get<TeacherDocument[]>(`${this.apiUrl}/teacher/${teacherId}`);
  }

  getDocument(id: number): Observable<TeacherDocument> {
    return this.http.get<TeacherDocument>(`${this.apiUrl}/${id}`);
  }

  downloadDocument(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/download`, {
      responseType: 'blob'
    });
  }

  deleteDocument(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  searchDocuments(request: DocumentSearchRequest): Observable<PagedResult<TeacherDocument>> {
    return this.http.post<PagedResult<TeacherDocument>>(`${this.apiUrl}/search`, request);
  }

  sendDocumentByEmail(documentId: number, request: SendDocumentEmailRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/${documentId}/send-email`, request);
  }

  // User document methods
  uploadMyDocument(file: File, documentType: string, customDocumentType: string, remarks: string): Observable<TeacherDocument> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('documentType', documentType);
    formData.append('customDocumentType', customDocumentType || '');
    formData.append('remarks', remarks || '');

    return this.http.post<TeacherDocument>(`${this.apiUrl}/upload-my-document`, formData);
  }

  getMyDocuments(): Observable<TeacherDocument[]> {
    return this.http.get<TeacherDocument[]>(`${this.apiUrl}/my-documents`);
  }
}
