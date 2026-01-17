import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PollType, QuestionType, CreatePoll, CreatePollQuestion, CreatePollOption } from '../../models/poll.model';
import { PollService } from '../../services/poll.service';

@Component({
  selector: 'app-poll-create',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './poll-create.component.html',
  styleUrls: ['./poll-create.component.css']
})
export class PollCreateComponent implements OnInit {
  poll: CreatePoll = {
    title: '',
    description: '',
    type: PollType.Survey,
    allowMultipleVotes: false,
    endDate: undefined,
    questions: []
  };

  loading = false;
  submitting = false;
  error: string | null = null;
  success = false;

  pollTypes = [
    { value: PollType.YesNo, label: 'Yes/No Poll' },
    { value: PollType.MultipleChoice, label: 'Multiple Choice' },
    { value: PollType.Survey, label: 'Survey' }
  ];

  questionTypes = [
    { value: QuestionType.YesNo, label: 'Yes/No' },
    { value: QuestionType.MultipleChoice, label: 'Multiple Choice' },
    { value: QuestionType.Checkbox, label: 'Checkbox' },
    { value: QuestionType.Text, label: 'Text' },
    { value: QuestionType.Rating, label: 'Rating (1-5)' }
  ];

  constructor(
    private router: Router,
    private pollService: PollService
  ) {}

  ngOnInit(): void {
    this.addQuestion();
  }

  addQuestion(): void {
    const newQuestion: CreatePollQuestion = {
      questionText: '',
      type: QuestionType.MultipleChoice,
      isRequired: true,
      options: []
    };

    if (newQuestion.type === QuestionType.MultipleChoice || newQuestion.type === QuestionType.Checkbox) {
      newQuestion.options = [
        { optionText: '' },
        { optionText: '' }
      ];
    }

    this.poll.questions.push(newQuestion);
  }

  removeQuestion(index: number): void {
    if (this.poll.questions.length > 1) {
      this.poll.questions.splice(index, 1);
    }
  }

  addOption(questionIndex: number): void {
    const question = this.poll.questions[questionIndex];
    if (question.options) {
      question.options.push({ optionText: '' });
    }
  }

  removeOption(questionIndex: number, optionIndex: number): void {
    const question = this.poll.questions[questionIndex];
    if (question.options && question.options.length > 2) {
      question.options.splice(optionIndex, 1);
    }
  }

  onQuestionTypeChange(questionIndex: number, type: QuestionType): void {
    const question = this.poll.questions[questionIndex];
    question.type = type;

    // Reset options based on question type
    if (type === QuestionType.MultipleChoice || type === QuestionType.Checkbox) {
      if (!question.options || question.options.length < 2) {
        question.options = [
          { optionText: '' },
          { optionText: '' }
        ];
      }
    } else {
      question.options = [];
    }
  }

  moveQuestionUp(index: number): void {
    if (index > 0) {
      const temp = this.poll.questions[index];
      this.poll.questions[index] = this.poll.questions[index - 1];
      this.poll.questions[index - 1] = temp;
    }
  }

  moveQuestionDown(index: number): void {
    if (index < this.poll.questions.length - 1) {
      const temp = this.poll.questions[index];
      this.poll.questions[index] = this.poll.questions[index + 1];
      this.poll.questions[index + 1] = temp;
    }
  }

  isFormValid(): boolean {
    // Basic validation
    if (!this.poll.title.trim()) return false;
    if (this.poll.questions.length === 0) return false;

    // Validate each question
    return this.poll.questions.every(question => {
      if (!question.questionText.trim()) return false;

      if (question.type === QuestionType.MultipleChoice || question.type === QuestionType.Checkbox) {
        if (!question.options || question.options.length < 2) return false;
        return question.options.every(option => option.optionText.trim());
      }

      return true;
    });
  }

  submitPoll(): void {
    if (!this.isFormValid()) {
      this.error = 'Please fill in all required fields correctly.';
      return;
    }

    this.submitting = true;
    this.error = null;

    // Clean up empty options
    this.poll.questions.forEach(question => {
      if (question.options) {
        question.options = question.options.filter(option => option.optionText.trim());
      }
    });

    this.pollService.createPoll(this.poll).subscribe({
      next: (poll) => {
        this.success = true;
        this.submitting = false;
        setTimeout(() => {
          this.router.navigate(['/polls', poll.id]);
        }, 2000);
      },
      error: (err) => {
        this.error = 'Failed to create poll. Please try again.';
        this.submitting = false;
        console.error('Error creating poll:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/polls']);
  }

  getPollTypeLabel(type: PollType): string {
    switch (type) {
      case PollType.YesNo: return 'Yes/No';
      case PollType.MultipleChoice: return 'Multiple Choice';
      case PollType.Survey: return 'Survey';
      default: return 'Unknown';
    }
  }

  getQuestionTypeLabel(type: QuestionType): string {
    switch (type) {
      case QuestionType.YesNo: return 'Yes/No';
      case QuestionType.MultipleChoice: return 'Multiple Choice';
      case QuestionType.Checkbox: return 'Checkbox';
      case QuestionType.Text: return 'Text';
      case QuestionType.Rating: return 'Rating';
      default: return 'Unknown';
    }
  }

  requiresOptions(type: QuestionType): boolean {
    return type === QuestionType.MultipleChoice || type === QuestionType.Checkbox;
  }

  trackByFn(index: number, item: any): number {
    return index;
  }

  trackOptionByFn(index: number, item: any): number {
    return index;
  }

  goToDashboard(): void {
    this.router.navigate(['/user-dashboard']);
  }
}
