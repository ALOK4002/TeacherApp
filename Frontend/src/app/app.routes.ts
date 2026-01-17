import { Routes } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { SchoolManagementComponent } from './components/school-management/school-management.component';
import { TeacherManagementComponent } from './components/teacher-management/teacher-management.component';
import { NoticeBoardComponent } from './components/notice-board/notice-board.component';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { SearchComponent } from './components/search/search.component';
import { TeacherReportComponent } from './components/teacher-report/teacher-report.component';
import { TeacherDocumentsComponent } from './components/teacher-documents/teacher-documents.component';
import { SelfDeclarationComponent } from './components/self-declaration/self-declaration.component';
import { MyDocumentsComponent } from './components/my-documents/my-documents.component';
import { MyActivityComponent } from './components/my-activity/my-activity.component';
import { PaymentResultComponent } from './components/payment-result/payment-result.component';
import { PaymentApprovalComponent } from './components/payment-approval/payment-approval.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { UserOnboardingComponent } from './components/user-onboarding/user-onboarding.component';
import { PollListComponent } from './components/poll-list/poll-list.component';
import { PollCreateComponent } from './components/poll-create/poll-create.component';
import { PollVoteComponent } from './components/poll-vote/poll-vote.component';
import { PollResultsComponent } from './components/poll-results/poll-results.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/register', pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'welcome', component: WelcomeComponent },
  
  // User Routes (Protected)
  { path: 'self-declaration', component: SelfDeclarationComponent, canActivate: [authGuard] },
  { path: 'my-documents', component: MyDocumentsComponent, canActivate: [authGuard] },
  { path: 'my-activity', component: MyActivityComponent, canActivate: [authGuard] },
  { path: 'user-dashboard', component: UserDashboardComponent, canActivate: [authGuard] },
  { path: 'payment-result', component: PaymentResultComponent, canActivate: [authGuard] },
  
  // Admin Routes (Protected)
  { path: 'user-onboarding', component: UserOnboardingComponent, canActivate: [authGuard, adminGuard] },
  { path: 'payment-approval', component: PaymentApprovalComponent, canActivate: [authGuard, adminGuard] },
  { path: 'schools', component: SchoolManagementComponent, canActivate: [authGuard, adminGuard] },
  { path: 'teachers', component: TeacherManagementComponent, canActivate: [authGuard, adminGuard] },
  { path: 'teacher-report', component: TeacherReportComponent, canActivate: [authGuard, adminGuard] },
  { path: 'teacher-documents/:id', component: TeacherDocumentsComponent, canActivate: [authGuard, adminGuard] },
  
  // Public Routes
  { path: 'notices', component: NoticeBoardComponent },
  { path: 'about', component: AboutUsComponent },
  { path: 'search', component: SearchComponent },
  
  // Poll Routes (Public)
  { path: 'polls', component: PollListComponent },
  { path: 'polls/create', component: PollCreateComponent, canActivate: [authGuard] },
  { path: 'polls/:id', component: PollVoteComponent },
  { path: 'polls/:id/results', component: PollResultsComponent },
  
  { path: '**', redirectTo: '/register' }
];
