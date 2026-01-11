import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="register-container ms-Flex ms-Flex--center">
      <div class="register-card ms-Card ms-Card--elevated ms-fadeIn">
        <div class="register-header">
          <div class="logo-section">
            <div class="education-logo">
              <span class="education-icon">📚</span>
            </div>
            <h1 class="ms-fontSize-28 ms-fontWeight-bold" style="color: var(--accent-education); margin: 12px 0 4px;">
              Join Bihar Teacher Portal
            </h1>
            <p class="ms-fontSize-14" style="color: var(--neutral-gray-90); margin: 0;">
              Create your account to connect with educators across Bihar
            </p>
          </div>
        </div>

        <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="register-form">
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
              [class.error]="registerForm.get('userNameOrEmail')?.invalid && registerForm.get('userNameOrEmail')?.touched"
            >
            <div class="error-message ms-fontSize-12" 
                 *ngIf="registerForm.get('userNameOrEmail')?.invalid && registerForm.get('userNameOrEmail')?.touched">
              <span class="education-icon">⚠️</span>
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
              placeholder="Enter your password (min 6 characters)"
              [class.error]="registerForm.get('password')?.invalid && registerForm.get('password')?.touched"
            >
            <div class="error-message ms-fontSize-12" 
                 *ngIf="registerForm.get('password')?.invalid && registerForm.get('password')?.touched">
              <span class="education-icon">⚠️</span>
              Password must be at least 6 characters long
            </div>
          </div>

          <div class="ms-TextField">
            <label class="ms-TextField-label" for="confirmPassword">
              <span class="education-icon">🔐</span>
              Confirm Password
            </label>
            <input 
              type="password" 
              id="confirmPassword"
              formControlName="confirmPassword" 
              class="ms-TextField-field"
              placeholder="Confirm your password"
              [class.error]="registerForm.get('confirmPassword')?.invalid && registerForm.get('confirmPassword')?.touched"
            >
            <div class="error-message ms-fontSize-12" 
                 *ngIf="registerForm.get('confirmPassword')?.invalid && registerForm.get('confirmPassword')?.touched">
              <span class="education-icon">⚠️</span>
              Passwords do not match
            </div>
          </div>

          <div class="error-message ms-fontSize-14" *ngIf="errorMessage" style="text-align: center; margin: 16px 0;">
            <span class="education-icon">❌</span>
            {{ errorMessage }}
          </div>

          <div class="success-message ms-fontSize-14" *ngIf="successMessage" style="text-align: center; margin: 16px 0;">
            <span class="education-icon">✅</span>
            {{ successMessage }}
          </div>

          <button 
            type="submit" 
            class="ms-Button ms-Button--knowledge" 
            [disabled]="registerForm.invalid || isLoading"
            style="width: 100%; margin-top: 16px;">
            <span class="education-icon" *ngIf="!isLoading">🎓</span>
            <span class="loading-spinner" *ngIf="isLoading">⏳</span>
            {{ isLoading ? 'Creating Account...' : 'Create Account' }}
          </button>
        </form>

        <div class="register-footer">
          <div class="divider">
            <span class="ms-fontSize-12" style="color: var(--neutral-gray-80);">Already have an account?</span>
          </div>
          <a routerLink="/login" class="ms-Button ms-Button--secondary" style="width: 100%; margin-top: 12px;">
            <span class="education-icon">🚀</span>
            Sign In
          </a>
        </div>

        <div class="educational-quote ms-Card" style="margin-top: 24px; background: var(--gradient-growth); color: white; text-align: center;">
          <p class="ms-fontSize-14" style="margin: 0; font-style: italic;">
            "The best teachers are those who show you where to look but don't tell you what to see."
          </p>
          <small class="ms-fontSize-12" style="opacity: 0.9;">- Alexandra K. Trenfor</small>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .register-container {
      min-height: 100vh;
      background: var(--gradient-growth);
      padding: var(--spacing-l);
    }

    .register-card {
      width: 100%;
      max-width: 420px;
      padding: var(--spacing-xxxl);
      background: var(--neutral-white);
      border-radius: var(--border-radius-xlarge);
      box-shadow: var(--shadow-64);
    }

    .register-header {
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
      background: var(--gradient-creativity);
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

    .register-form {
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

    .success-message {
      color: var(--success-color);
      margin-top: var(--spacing-xs);
      display: flex;
      align-items: center;
      gap: var(--spacing-xs);
      justify-content: center;
    }

    .register-footer {
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
      .register-card {
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
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      userNameOrEmail: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    
    return null;
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const { userNameOrEmail, password } = this.registerForm.value;

      this.authService.register({ userNameOrEmail, password }).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.successMessage = 'Registration successful! Redirecting to login...';
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
        }
      });
    }
  }
}