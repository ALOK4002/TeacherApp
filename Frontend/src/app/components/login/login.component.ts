import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UserProfileService } from '../../services/user-profile.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="login-container ms-Flex ms-Flex--center">
      <div class="login-card ms-Card ms-Card--elevated ms-fadeIn">
        <div class="login-header">
          <div class="logo-section">
            <div class="education-logo">
              <span class="education-icon">🎓</span>
            </div>
            <h1 class="ms-fontSize-28 ms-fontWeight-bold" style="color: var(--primary-color); margin: 12px 0 4px;">
              Bihar Teacher Portal
            </h1>
            <p class="ms-fontSize-14" style="color: var(--neutral-gray-90); margin: 0;">
              Empowering Education Through Digital Innovation
            </p>
          </div>
        </div>

        <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="login-form">
          <div class="ms-TextField">
            <label class="ms-TextField-label" for="userNameOrEmail">
              <span class="education-icon">👤</span>
              Username or Email
            </label>
            <input 
              type="text" 
              id="userNameOrEmail"
              formControlName="userNameOrEmail" 
              class="ms-TextField-field"
              placeholder="Enter your username or email"
              [class.error]="loginForm.get('userNameOrEmail')?.invalid && loginForm.get('userNameOrEmail')?.touched"
            >
            <div class="error-message ms-fontSize-12" 
                 *ngIf="loginForm.get('userNameOrEmail')?.invalid && loginForm.get('userNameOrEmail')?.touched">
              Username or Email is required
            </div>
          </div>

          <div class="ms-TextField">
            <label class="ms-TextField-label" for="password">
              <span class="education-icon">🔒</span>
              Password
            </label>
            <input 
              type="password" 
              id="password"
              formControlName="password" 
              class="ms-TextField-field"
              placeholder="Enter your password"
              [class.error]="loginForm.get('password')?.invalid && loginForm.get('password')?.touched"
            >
            <div class="error-message ms-fontSize-12" 
                 *ngIf="loginForm.get('password')?.invalid && loginForm.get('password')?.touched">
              Password is required
            </div>
          </div>

          <div class="error-message ms-fontSize-14" *ngIf="errorMessage" style="text-align: center; margin: 16px 0;">
            <span class="education-icon">⚠️</span>
            {{ errorMessage }}
          </div>

          <button 
            type="submit" 
            class="ms-Button ms-Button--education" 
            [disabled]="loginForm.invalid || isLoading"
            style="width: 100%; margin-top: 16px;">
            <span class="education-icon" *ngIf="!isLoading">🚀</span>
            <span class="loading-spinner" *ngIf="isLoading">⏳</span>
            {{ isLoading ? 'Signing In...' : 'Sign In' }}
          </button>
        </form>

        <div class="login-footer">
          <div class="divider">
            <span class="ms-fontSize-12" style="color: var(--neutral-gray-80);">New to Bihar Teacher Portal?</span>
          </div>
          <a routerLink="/register" class="ms-Button ms-Button--secondary" style="width: 100%; margin-top: 12px;">
            <span class="education-icon">📝</span>
            Create Account
          </a>
        </div>

        <div class="educational-quote ms-Card" style="margin-top: 24px; background: var(--gradient-knowledge); color: white; text-align: center;">
          <p class="ms-fontSize-14" style="margin: 0; font-style: italic;">
            "Education is the most powerful weapon which you can use to change the world."
          </p>
          <small class="ms-fontSize-12" style="opacity: 0.9;">- Nelson Mandela</small>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      min-height: 100vh;
      background: var(--gradient-education);
      padding: var(--spacing-l);
    }

    .login-card {
      width: 100%;
      max-width: 420px;
      padding: var(--spacing-xxxl);
      background: var(--neutral-white);
      border-radius: var(--border-radius-xlarge);
      box-shadow: var(--shadow-64);
    }

    .login-header {
      text-align: center;
      margin-bottom: var(--spacing-xxxl);
    }

    .logo-section {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .education-logo {
      width: 80px;
      height: 80px;
      background: var(--gradient-knowledge);
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: var(--spacing-l);
      box-shadow: var(--shadow-8);
    }

    .education-icon {
      font-size: var(--font-size-32);
    }

    .login-form {
      display: flex;
      flex-direction: column;
      gap: var(--spacing-l);
    }

    .ms-TextField-field.error {
      border-color: var(--error-color);
      box-shadow: 0 0 0 1px var(--error-color);
    }

    .error-message {
      color: var(--error-color);
      margin-top: var(--spacing-xs);
      display: flex;
      align-items: center;
      gap: var(--spacing-xs);
    }

    .login-footer {
      margin-top: var(--spacing-xxl);
      text-align: center;
    }

    .divider {
      margin-bottom: var(--spacing-m);
      padding: var(--spacing-m) 0;
      border-top: 1px solid var(--neutral-gray-30);
    }

    .educational-quote {
      padding: var(--spacing-l);
      border-radius: var(--border-radius-large);
    }

    .loading-spinner {
      animation: spin 1s linear infinite;
    }

    @keyframes spin {
      from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
    }

    /* Responsive Design */
    @media (max-width: 480px) {
      .login-card {
        padding: var(--spacing-xxl);
        margin: var(--spacing-l);
      }
      
      .education-logo {
        width: 60px;
        height: 60px;
      }
      
      .education-icon {
        font-size: var(--font-size-24);
      }
    }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userProfileService: UserProfileService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      userNameOrEmail: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const loginRequest = {
        userName: this.loginForm.value.userNameOrEmail,
        password: this.loginForm.value.password
      };

      this.authService.login(loginRequest).subscribe({
        next: (response) => {
          // Check if user is approved
          if (!response.isApproved) {
            this.isLoading = false;
            this.errorMessage = 'Your account is pending admin approval. Please wait for approval before logging in.';
            this.authService.logout();
            return;
          }

          // Role-based redirect
          if (this.authService.isAdmin()) {
            // Admin goes to teacher management
            this.isLoading = false;
            this.router.navigate(['/teachers']);
          } else if (this.authService.isTeacher()) {
            // Teacher: check if profile exists
            this.userProfileService.hasProfile().subscribe({
              next: (hasProfile) => {
                this.isLoading = false;
                if (hasProfile) {
                  this.router.navigate(['/user-dashboard']);
                } else {
                  this.router.navigate(['/self-declaration']);
                }
              },
              error: (error) => {
                this.isLoading = false;
                // If error checking profile, assume no profile and go to self-declaration
                this.router.navigate(['/self-declaration']);
              }
            });
          } else {
            // Default fallback
            this.isLoading = false;
            this.router.navigate(['/welcome']);
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Login failed. Please check your credentials.';
        }
      });
    }
  }
}