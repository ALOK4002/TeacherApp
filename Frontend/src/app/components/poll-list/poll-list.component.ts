import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Poll } from '../../models/poll.model';
import { PollService } from '../../services/poll.service';

@Component({
  selector: 'app-poll-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './poll-list.component.html',
  styleUrls: ['./poll-list.component.css']
})
export class PollListComponent implements OnInit {
  polls: Poll[] = [];
  loading = false;
  error: string | null = null;

  constructor(private pollService: PollService, private router: Router) {}

  ngOnInit(): void {
    this.loadPolls();
  }

  loadPolls(): void {
    this.loading = true;
    this.error = null;
    
    this.pollService.getAllActivePolls().subscribe({
      next: (polls) => {
        this.polls = polls;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load polls. Please try again.';
        this.loading = false;
        console.error('Error loading polls:', err);
      }
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  getPollTypeLabel(type: number): string {
    switch (type) {
      case 1: return 'Yes/No';
      case 2: return 'Multiple Choice';
      case 3: return 'Survey';
      default: return 'Unknown';
    }
  }

  isPollExpired(poll: Poll): boolean {
    if (!poll.endDate) return false;
    return new Date(poll.endDate) < new Date();
  }

  goToDashboard(): void {
    this.router.navigate(['/user-dashboard']);
  }
}
