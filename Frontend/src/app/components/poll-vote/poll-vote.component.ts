import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Poll, SubmitPollResponse, SubmitPollAnswer, PollResponse } from '../../models/poll.model';
import { PollService } from '../../services/poll.service';

@Component({
  selector: 'app-poll-vote',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, DatePipe],
  templateUrl: './poll-vote.component.html',
  styleUrls: ['./poll-vote.component.css']
})
export class PollVoteComponent implements OnInit {
  poll: Poll | null = null;
  loading = false;
  submitting = false;
  error: string | null = null;
  success = false;
  userResponse: PollResponse | null = null;
  
  // Form state
  answers: { [questionId: number]: any } = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private pollService: PollService
  ) {}

  ngOnInit(): void {
    const pollId = this.route.snapshot.paramMap.get('id');
    if (pollId) {
      this.loadPoll(+pollId);
      this.checkUserResponse(+pollId);
    }
  }

  loadPoll(id: number): void {
    this.loading = true;
    this.error = null;

    this.pollService.getPollById(id).subscribe({
      next: (poll) => {
        this.poll = poll;
        this.loading = false;
        this.initializeAnswers();
      },
      error: (err) => {
        this.error = 'Failed to load poll. Please try again.';
        this.loading = false;
        console.error('Error loading poll:', err);
      }
    });
  }

  checkUserResponse(id: number): void {
    this.pollService.getUserPollResponse(id).subscribe({
      next: (response) => {
        this.userResponse = response;
        if (response) {
          this.loadUserAnswers(response);
        }
      },
      error: (err) => {
        console.error('Error checking user response:', err);
      }
    });
  }

  initializeAnswers(): void {
    if (!this.poll) return;

    this.answers = {};
    this.poll.questions.forEach(question => {
      if (question.type === 1) { // Yes/No
        this.answers[question.id] = null;
      } else if (question.type === 2) { // Multiple Choice
        this.answers[question.id] = null;
      } else if (question.type === 3) { // Checkbox
        this.answers[question.id] = [];
      } else if (question.type === 4) { // Text
        this.answers[question.id] = '';
      } else if (question.type === 5) { // Rating
        this.answers[question.id] = 0;
      }
    });
  }

  loadUserAnswers(response: PollResponse): void {
    response.answers.forEach(answer => {
      if (answer.pollOptionId) {
        if (this.poll?.questions.find(q => q.id === answer.pollQuestionId)?.type === 3) {
          // Checkbox
          if (!this.answers[answer.pollQuestionId]) {
            this.answers[answer.pollQuestionId] = [];
          }
          this.answers[answer.pollQuestionId].push(answer.pollOptionId);
        } else {
          // Multiple Choice or Yes/No
          this.answers[answer.pollQuestionId] = answer.pollOptionId;
        }
      } else if (answer.textAnswer) {
        this.answers[answer.pollQuestionId] = answer.textAnswer;
      } else if (answer.ratingValue) {
        this.answers[answer.pollQuestionId] = answer.ratingValue;
      }
    });
  }

  onYesNoChange(questionId: number, value: boolean): void {
    this.answers[questionId] = value;
  }

  onMultipleChoiceChange(questionId: number, optionId: number): void {
    this.answers[questionId] = optionId;
  }

  onCheckboxChange(questionId: number, optionId: number, checked: boolean): void {
    if (!this.answers[questionId]) {
      this.answers[questionId] = [];
    }
    const current = this.answers[questionId] as number[];
    
    if (checked) {
      current.push(optionId);
    } else {
      const index = current.indexOf(optionId);
      if (index > -1) {
        current.splice(index, 1);
      }
    }
  }

  onTextChange(questionId: number, value: string): void {
    this.answers[questionId] = value;
  }

  onRatingChange(questionId: number, value: number): void {
    this.answers[questionId] = value;
  }

  isFormValid(): boolean {
    if (!this.poll) return false;

    return this.poll.questions.every(question => {
      const answer = this.answers[question.id];
      
      if (!question.isRequired) return true;
      
      if (question.type === 3) { // Checkbox
        return Array.isArray(answer) && answer.length > 0;
      }
      
      return answer !== null && answer !== undefined && answer !== '';
    });
  }

  submitResponse(): void {
    if (!this.poll || !this.isFormValid()) return;

    this.submitting = true;
    this.error = null;

    const submitAnswers: SubmitPollAnswer[] = this.poll.questions.map(question => {
      const answer = this.answers[question.id];
      const submitAnswer: SubmitPollAnswer = {
        pollQuestionId: question.id
      };

      if (question.type === 1 || question.type === 2) { // Yes/No or Multiple Choice
        submitAnswer.pollOptionId = answer;
      } else if (question.type === 3) { // Checkbox
        // For checkbox, we'll submit each selected option as a separate answer
        return null;
      } else if (question.type === 4) { // Text
        submitAnswer.textAnswer = answer;
      } else if (question.type === 5) { // Rating
        submitAnswer.ratingValue = answer;
      }

      return submitAnswer;
    }).filter(answer => answer !== null) as SubmitPollAnswer[];

    // Handle checkbox questions (multiple options)
    this.poll.questions.forEach(question => {
      if (question.type === 3) { // Checkbox
        const selectedOptions = this.answers[question.id] as number[];
        if (selectedOptions && selectedOptions.length > 0) {
          selectedOptions.forEach(optionId => {
            submitAnswers.push({
              pollQuestionId: question.id,
              pollOptionId: optionId
            });
          });
        }
      }
    });

    const submitResponse: SubmitPollResponse = {
      pollId: this.poll.id,
      answers: submitAnswers
    };

    this.pollService.submitPollResponse(this.poll.id, submitResponse).subscribe({
      next: (response) => {
        this.success = true;
        this.submitting = false;
        this.userResponse = response;
      },
      error: (err) => {
        this.error = 'Failed to submit your response. Please try again.';
        this.submitting = false;
        console.error('Error submitting response:', err);
      }
    });
  }

  viewResults(): void {
    if (this.poll) {
      this.router.navigate(['/polls', this.poll.id, 'results']);
    }
  }

  goBack(): void {
    this.router.navigate(['/polls']);
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

  hasUserResponded(): boolean {
    return this.userResponse !== null;
  }

  isPollExpired(): boolean {
    if (!this.poll?.endDate) return false;
    return new Date(this.poll.endDate) < new Date();
  }

  goToDashboard(): void {
    this.router.navigate(['/user-dashboard']);
  }
}
