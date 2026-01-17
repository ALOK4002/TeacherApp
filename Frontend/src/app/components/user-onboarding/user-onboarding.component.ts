import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.models';

@Component({
  selector: 'app-user-onboarding',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-onboarding.component.html',
  styleUrls: ['./user-onboarding.component.css']
})
export class UserOnboardingComponent implements OnInit {
  pendingUsers: User[] = [];
  allUsers: User[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  showRejectDialog = false;
  selectedUserId: number | null = null;
  rejectForm: FormGroup;
  activeTab: 'pending' | 'all' = 'pending';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.rejectForm = this.fb.group({
      rejectionReason: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  ngOnInit() {
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/user-dashboard']);
      return;
    }
    this.loadPendingUsers();
    this.loadAllUsers();
  }

  loadPendingUsers() {
    this.isLoading = true;
    this.authService.getPendingUsers().subscribe({
      next: (response) => {
        this.pendingUsers = response.pendingUsers;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading pending users:', error);
        this.errorMessage = 'Failed to load pending users';
        this.isLoading = false;
      }
    });
  }

  loadAllUsers() {
    this.authService.getAllUsers().subscribe({
      next: (users) => {
        this.allUsers = users;
      },
      error: (error) => {
        console.error('Error loading all users:', error);
      }
    });
  }

  approveUser(userId: number) {
    if (confirm('Are you sure you want to approve this user?')) {
      this.authService.approveUser(userId).subscribe({
        next: () => {
          this.successMessage = 'User approved successfully!';
          this.loadPendingUsers();
          this.loadAllUsers();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to approve user';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  openRejectDialog(userId: number) {
    this.selectedUserId = userId;
    this.showRejectDialog = true;
    this.rejectForm.reset();
  }

  closeRejectDialog() {
    this.showRejectDialog = false;
    this.selectedUserId = null;
  }

  rejectUser() {
    if (this.rejectForm.valid && this.selectedUserId) {
      const rejectionReason = this.rejectForm.value.rejectionReason;
      
      this.authService.rejectUser(this.selectedUserId, rejectionReason).subscribe({
        next: () => {
          this.successMessage = 'User rejected successfully!';
          this.closeRejectDialog();
          this.loadPendingUsers();
          this.loadAllUsers();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to reject user';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  switchTab(tab: 'pending' | 'all') {
    this.activeTab = tab;
  }

  getUserStatusBadge(user: User): string {
    if (!user.isApproved) return 'pending';
    if (!user.isActive) return 'inactive';
    return 'approved';
  }

  getUserStatusLabel(user: User): string {
    if (!user.isApproved) return 'Pending';
    if (!user.isActive) return 'Inactive';
    return 'Approved';
  }

  navigateToPaymentApproval() {
    this.router.navigate(['/payment-approval']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
