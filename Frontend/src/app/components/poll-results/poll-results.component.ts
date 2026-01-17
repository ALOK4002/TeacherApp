import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Poll, PollResult } from '../../models/poll.model';
import { PollService } from '../../services/poll.service';

@Component({
  selector: 'app-poll-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './poll-results.component.html',
  styleUrls: ['./poll-results.component.css']
})
export class PollResultsComponent implements OnInit {
  poll: Poll | null = null;
  pollResult: PollResult | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private pollService: PollService
  ) {}

  ngOnInit(): void {
    const pollId = this.route.snapshot.paramMap.get('id');
    if (pollId) {
      this.loadPollResults(+pollId);
    }
  }

  loadPollResults(id: number): void {
    this.loading = true;
    this.error = null;

    // Load poll details and results in parallel
    this.pollService.getPollById(id).subscribe({
      next: (poll) => {
        this.poll = poll;
      },
      error: (err) => {
        this.error = 'Failed to load poll details.';
        this.loading = false;
        console.error('Error loading poll:', err);
      }
    });

    this.pollService.getPollResults(id).subscribe({
      next: (result) => {
        this.pollResult = result;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load poll results.';
        this.loading = false;
        console.error('Error loading poll results:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/polls']);
  }

  voteAgain(): void {
    if (this.poll) {
      this.router.navigate(['/polls', this.poll.id]);
    }
  }

  getQuestionTypeLabel(type: number): string {
    switch (type) {
      case 1: return 'Yes/No';
      case 2: return 'Multiple Choice';
      case 3: return 'Checkbox';
      case 4: return 'Text';
      case 5: return 'Rating';
      default: return 'Unknown';
    }
  }

  getPollTypeLabel(type: number): string {
    switch (type) {
      case 1: return 'Yes/No';
      case 2: return 'Multiple Choice';
      case 3: return 'Survey';
      default: return 'Unknown';
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  getPercentageColor(percentage: number): string {
    if (percentage >= 70) return '#28a745';
    if (percentage >= 40) return '#ffc107';
    return '#dc3545';
  }

  getRatingStars(rating: number): string {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    let stars = '';
    
    for (let i = 0; i < fullStars; i++) {
      stars += '★';
    }
    if (hasHalfStar && fullStars < 5) {
      stars += '☆';
    }
    for (let i = stars.length; i < 5; i++) {
      stars += '☆';
    }
    
    return stars;
  }

  goToDashboard(): void {
    this.router.navigate(['/user-dashboard']);
  }
}
