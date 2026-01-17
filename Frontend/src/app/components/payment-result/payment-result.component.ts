import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PaymentStatus } from '../../models/payment.models';

@Component({
  selector: 'app-payment-result',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-result.component.html',
  styleUrls: ['./payment-result.component.css']
})
export class PaymentResultComponent implements OnInit {
  status: string = '';
  orderId: string = '';
  error: string = '';
  isSuccess = false;
  isPending = false;
  isFailed = false;

  PaymentStatus = PaymentStatus;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.status = params['status'] || '';
      this.orderId = params['orderId'] || '';
      this.error = params['error'] || '';

      this.isSuccess = this.status === 'Success' || this.status === PaymentStatus.Success.toString();
      this.isPending = this.status === 'PendingApproval' || this.status === PaymentStatus.PendingApproval.toString();
      this.isFailed = this.status === 'Failed' || this.status === 'failed' || this.status === PaymentStatus.Failed.toString();
    });
  }

  goToDashboard() {
    this.router.navigate(['/user-dashboard']);
  }

  goToDocuments() {
    this.router.navigate(['/my-documents']);
  }

  goToActivity() {
    this.router.navigate(['/my-activity']);
  }
}