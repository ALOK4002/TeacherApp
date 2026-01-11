import { Routes } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { SchoolManagementComponent } from './components/school-management/school-management.component';
import { TeacherManagementComponent } from './components/teacher-management/teacher-management.component';
import { NoticeBoardComponent } from './components/notice-board/notice-board.component';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { SearchComponent } from './components/search/search.component';

export const routes: Routes = [
  { path: '', redirectTo: '/register', pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'welcome', component: WelcomeComponent },
  { path: 'schools', component: SchoolManagementComponent },
  { path: 'teachers', component: TeacherManagementComponent },
  { path: 'notices', component: NoticeBoardComponent },
  { path: 'about', component: AboutUsComponent },
  { path: 'search', component: SearchComponent },
  { path: '**', redirectTo: '/register' }
];
