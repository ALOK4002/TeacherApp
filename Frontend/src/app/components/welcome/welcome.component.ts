import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="welcome-container">
      <div class="welcome-card">
        <h1>Welcome!</h1>
        <p class="welcome-message">
          Welcome, <strong>{{ userName }}</strong>! You have successfully logged in.
        </p>
        <div class="action-buttons">
          <a routerLink="/user-onboarding" class="btn-primary" *ngIf="isAdmin">👥 User Onboarding</a>
          <a routerLink="/schools" class="btn-primary" *ngIf="isAdmin">🏫 Manage Schools</a>
          <a routerLink="/teachers" class="btn-primary" *ngIf="isAdmin">👨‍🏫 Manage Teachers</a>
          <a routerLink="/teacher-report" class="btn-primary" *ngIf="isAdmin">📊 Teacher Report</a>
          <a routerLink="/user-dashboard" class="btn-primary" *ngIf="!isAdmin">🏠 My Dashboard</a>
          <a routerLink="/my-documents" class="btn-primary" *ngIf="!isAdmin">📄 My Documents</a>
          <a routerLink="/notices" class="btn-primary">📢 Notice Board</a>
          <a routerLink="/about" class="btn-primary">ℹ️ About Us</a>
          <button (click)="logout()" class="btn-logout">🚪 Logout</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .welcome-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background-color: #f5f5f5;
    }

    .welcome-card {
      background: white;
      padding: 3rem;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
      text-align: center;
      max-width: 500px;
      width: 100%;
    }

    h1 {
      color: #28a745;
      margin-bottom: 1.5rem;
      font-size: 2.5rem;
    }

    .welcome-message {
      font-size: 1.2rem;
      color: #333;
      margin-bottom: 2rem;
      line-height: 1.6;
    }

    .action-buttons {
      display: flex;
      gap: 1rem;
      justify-content: center;
      flex-wrap: wrap;
    }

    .btn-primary {
      padding: 0.75rem 2rem;
      background-color: #007bff;
      color: white;
      text-decoration: none;
      border-radius: 4px;
      font-size: 1rem;
      transition: background-color 0.3s;
      display: inline-block;
    }

    .btn-primary:hover {
      background-color: #0056b3;
    }

    .btn-logout {
      padding: 0.75rem 2rem;
      background-color: #dc3545;
      color: white;
      border: none;
      border-radius: 4px;
      font-size: 1rem;
      cursor: pointer;
      transition: background-color 0.3s;
    }

    .btn-logout:hover {
      background-color: #c82333;
    }
  `]
})
export class WelcomeComponent implements OnInit {
  userName: string = '';
  isAdmin: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
    this.isAdmin = this.authService.isAdmin();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}