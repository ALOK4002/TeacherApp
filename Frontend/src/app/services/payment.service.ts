import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Payment, PaymentCallback } from '../models/payment.models';
import { CreatePaymentOrder, PaymentOrderResponse } from '../models/subscription.models';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = `${environment.apiUrl}/payment`;

  constructor(private http: HttpClient) {}

  createPaymentOrder(order: CreatePaymentOrder): Observable<PaymentOrderResponse> {
    return this.http.post<PaymentOrderResponse>(`${this.apiUrl}/create-order`, order);
  }

  getMyPayments(): Observable<Payment[]> {
    return this.http.get<Payment[]>(`${this.apiUrl}/my-payments`);
  }

  getPendingPayments(): Observable<Payment[]> {
    return this.http.get<Payment[]>(`${this.apiUrl}/pending`);
  }

  approvePayment(paymentId: number): Observable<Payment> {
    return this.http.post<Payment>(`${this.apiUrl}/approve/${paymentId}`, {});
  }

  rejectPayment(paymentId: number, rejectionReason: string): Observable<Payment> {
    return this.http.post<Payment>(`${this.apiUrl}/reject/${paymentId}`, { rejectionReason });
  }

  initiatePaytmPayment(orderResponse: PaymentOrderResponse): void {
    // Create a form and submit to Paytm
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = orderResponse.paytmUrl;

    const fields = {
      'MID': orderResponse.merchantId,
      'ORDER_ID': orderResponse.orderId,
      'TXN_AMOUNT': orderResponse.amount.toString(),
      'CUST_ID': 'USER_' + Date.now(),
      'INDUSTRY_TYPE_ID': 'Retail',
      'WEBSITE': 'DEFAULT',
      'CHANNEL_ID': 'WEB',
      'CALLBACK_URL': orderResponse.callbackUrl,
      'CHECKSUMHASH': orderResponse.checksumHash
    };

    Object.keys(fields).forEach(key => {
      const input = document.createElement('input');
      input.type = 'hidden';
      input.name = key;
      input.value = (fields as any)[key];
      form.appendChild(input);
    });

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
  }
}