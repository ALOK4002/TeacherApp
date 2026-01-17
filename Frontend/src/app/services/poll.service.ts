import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { 
  Poll, 
  CreatePoll, 
  UpdatePoll, 
  PollResponse, 
  SubmitPollResponse, 
  PollResult 
} from '../models/poll.model';

@Injectable({
  providedIn: 'root'
})
export class PollService {
  private readonly apiUrl = `${environment.apiUrl}/poll`;

  constructor(private http: HttpClient) {}

  getAllActivePolls(): Observable<Poll[]> {
    return this.http.get<Poll[]>(this.apiUrl);
  }

  getPollById(id: number): Observable<Poll> {
    return this.http.get<Poll>(`${this.apiUrl}/${id}`);
  }

  createPoll(poll: CreatePoll): Observable<Poll> {
    return this.http.post<Poll>(this.apiUrl, poll);
  }

  updatePoll(poll: UpdatePoll): Observable<Poll> {
    return this.http.put<Poll>(`${this.apiUrl}/${poll.id}`, poll);
  }

  deletePoll(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getUserPolls(): Observable<Poll[]> {
    return this.http.get<Poll[]>(`${this.apiUrl}/my-polls`);
  }

  getPollResults(id: number): Observable<PollResult> {
    return this.http.get<PollResult>(`${this.apiUrl}/${id}/results`);
  }

  submitPollResponse(pollId: number, response: SubmitPollResponse): Observable<PollResponse> {
    return this.http.post<PollResponse>(`${this.apiUrl}/${pollId}/respond`, response);
  }

  getUserPollResponse(id: number): Observable<PollResponse | null> {
    return this.http.get<PollResponse | null>(`${this.apiUrl}/${id}/my-response`);
  }
}
