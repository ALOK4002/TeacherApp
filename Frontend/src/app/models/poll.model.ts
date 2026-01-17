export enum PollType {
  YesNo = 1,
  MultipleChoice = 2,
  Survey = 3
}

export enum QuestionType {
  YesNo = 1,
  MultipleChoice = 2,
  Checkbox = 3,
  Text = 4,
  Rating = 5
}

export interface PollOption {
  id: number;
  optionText: string;
  order: number;
  voteCount: number;
}

export interface PollQuestion {
  id: number;
  questionText: string;
  type: QuestionType;
  order: number;
  isRequired: boolean;
  options: PollOption[];
}

export interface Poll {
  id: number;
  title: string;
  description?: string;
  type: PollType;
  allowMultipleVotes: boolean;
  createdDate: string;
  updatedDate: string;
  endDate?: string;
  createdByUserId: number;
  createdByUserName?: string;
  questions: PollQuestion[];
}

export interface CreatePollOption {
  optionText: string;
}

export interface CreatePollQuestion {
  questionText: string;
  type: QuestionType;
  isRequired: boolean;
  options?: CreatePollOption[];
}

export interface CreatePoll {
  title: string;
  description?: string;
  type: PollType;
  allowMultipleVotes: boolean;
  endDate?: string;
  questions: CreatePollQuestion[];
}

export interface UpdatePoll {
  id: number;
  title: string;
  description?: string;
  type: PollType;
  allowMultipleVotes: boolean;
  endDate?: string;
}

export interface PollAnswer {
  id: number;
  pollQuestionId: number;
  pollOptionId?: number;
  textAnswer?: string;
  ratingValue?: number;
}

export interface PollResponse {
  id: number;
  pollId: number;
  userId?: number;
  respondedDate: string;
  answers: PollAnswer[];
}

export interface SubmitPollAnswer {
  pollQuestionId: number;
  pollOptionId?: number;
  textAnswer?: string;
  ratingValue?: number;
}

export interface SubmitPollResponse {
  pollId: number;
  answers: SubmitPollAnswer[];
}

export interface PollOptionResult {
  id: number;
  optionText: string;
  order: number;
  voteCount: number;
  percentage: number;
}

export interface PollQuestionResult {
  id: number;
  questionText: string;
  type: QuestionType;
  order: number;
  isRequired: boolean;
  options: PollOptionResult[];
  textAnswers: string[];
  ratingAverage?: number;
}

export interface PollResult {
  pollId: number;
  title: string;
  description?: string;
  type: PollType;
  totalResponses: number;
  questions: PollQuestionResult[];
}
