import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { PaymentService } from '../../services/payment.service';
import { AuthService } from '../../services/auth.service';
import { Payment } from '../../models/payment.models';

@Component({
  selector: 'app-payment-approval',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-approval.component.html',
  styleUrls: ['./payment-approval.component.css']
})
export class PaymentApprovalComponent implements OnInit {
  payments: Payment[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private paymentService: PaymentService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadPendingPayments();
  }

  loadPendingPayments() {
    this.loading = true;
    this.error = null;
    
    this.paymentService.getPendingPayments().subscribe({
      next: (payments) => {
        this.payments = payments;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load pending payments';
        this.loading = false;
        console.error('Error loading payments:', error);
      }
    });
  }

  approvePayment(paymentId: number) {
    if (!confirm('Are you sure you want to approve this payment?')) {
      return;
    }

    this.paymentService.approvePayment(paymentId).subscribe({
      next: (response) => {
        alert('Payment approved successfully!');
        this.loadPendingPayments(); // Reload the list
      },
      error: (error) => {
        alert('Failed to approve payment');
        console.error('Error approving payment:', error);
      }
    });
  }

  rejectPayment(paymentId: number) {
    const reason = prompt('Please provide a reason for rejection:');
    if (!reason) {
      return;
    }

    this.paymentService.rejectPayment(paymentId, reason).subscribe({
      next: (response) => {
        alert('Payment rejected successfully!');
        this.loadPendingPayments(); // Reload the list
      },
      error: (error) => {
        alert('Failed to reject payment');
        console.error('Error rejecting payment:', error);
      }
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatAmount(amount: number): string {
    return `₹${amount.toFixed(2)}`;
  }

  navigateToUserOnboarding() {
    this.router.navigate(['/user-onboarding']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}