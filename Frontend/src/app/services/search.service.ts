import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SearchRequest {
  query: string;
  filters?: string[];
  facets?: string[];
  orderBy?: string;
  skip?: number;
  top?: number;
  includeTotalCount?: boolean;
}

export interface UnifiedSearchRequest {
  query: string;
  searchTypes?: string[];
  filters?: string[];
  skip?: number;
  top?: number;
}

export interface SearchResponse<T> {
  results: T[];
  totalCount?: number;
  facets?: { [key: string]: FacetResult[] };
  continuationToken?: string;
}

export interface FacetResult {
  value: any;
  count?: number;
}

export interface UnifiedSearchResult {
  type: string;
  id: string;
  title: string;
  description: string;
  category: string;
  date: string;
  score: number;
  data: any;
}

export interface UnifiedSearchResponse {
  results: UnifiedSearchResult[];
  totalCount: number;
  typeCounts: { [key: string]: number };
}

export interface TeacherSearchDocument {
  id: string;
  name: string;
  email: string;
  address: string;
  district: string;
  pincode: string;
  schoolName: string;
  class: string;
  createdDate: string;
  schoolId: number;
}

export interface SchoolSearchDocument {
  id: string;
  name: string;
  district: string;
  address: string;
  pincode: string;
  type: string;
  isActive: boolean;
  establishedDate: string;
  teacherCount: number;
}

export interface NoticeSearchDocument {
  id: string;
  title: string;
  message: string;
  category: string;
  priority: string;
  postedByUserName: string;
  postedDate: string;
  isActive: boolean;
  replyCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = `${environment.apiUrl}/search`;

  constructor(private http: HttpClient) {}

  // Unified search across all entities
  unifiedSearch(request: UnifiedSearchRequest): Observable<UnifiedSearchResponse> {
    return this.http.post<UnifiedSearchResponse>(`${this.apiUrl}/unified`, request);
  }

  // Search teachers
  searchTeachers(request: SearchRequest): Observable<SearchResponse<TeacherSearchDocument>> {
    return this.http.post<SearchResponse<TeacherSearchDocument>>(`${this.apiUrl}/teachers`, request);
  }

  // Search schools
  searchSchools(request: SearchRequest): Observable<SearchResponse<SchoolSearchDocument>> {
    return this.http.post<SearchResponse<SchoolSearchDocument>>(`${this.apiUrl}/schools`, request);
  }

  // Search notices
  searchNotices(request: SearchRequest): Observable<SearchResponse<NoticeSearchDocument>> {
    return this.http.post<SearchResponse<NoticeSearchDocument>>(`${this.apiUrl}/notices`, request);
  }

  // Get search suggestions
  getSuggestions(query: string, type: string = 'teachers'): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/suggestions`, {
      params: { query, type }
    });
  }

  // Get autocomplete suggestions
  getAutocomplete(query: string, type: string = 'teachers'): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/autocomplete`, {
      params: { query, type }
    });
  }

  // Initialize search indexes (Admin)
  initializeIndexes(): Observable<any> {
    return this.http.post(`${this.apiUrl}/initialize`, {});
  }

  // Sync data to search indexes (Admin)
  syncData(type?: string): Observable<any> {
    if (type) {
      return this.http.post(`${this.apiUrl}/sync`, {}, { params: { type } });
    } else {
      return this.http.post(`${this.apiUrl}/sync`, {});
    }
  }

  // Get search statistics
  getSearchStats(): Observable<any> {
    return this.http.get(`${this.apiUrl}/stats`);
  }
}