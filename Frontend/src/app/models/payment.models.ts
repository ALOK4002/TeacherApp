export interface Payment {
  id: number;
  userId: number;
  orderId: string;
  transactionId: string;
  amount: number;
  currency: string;
  status: PaymentStatus;
  gateway: PaymentGateway;
  paymentDate?: string;
  approvedDate?: string;
  rejectionReason?: string;
  createdDate: string;
  userName?: string;
  userEmail?: string;
  paymentMethod?: string;
  approvedByUserId?: number;
}

export enum PaymentStatus {
  Pending = 0,
  Success = 1,
  Failed = 2,
  Cancelled = 3,
  PendingApproval = 4,
  Approved = 5,
  Rejected = 6
}

export enum PaymentGateway {
  Paytm = 0,
  Razorpay = 1,
  UPI = 2
}

export interface PaymentCallback {
  orderId: string;
  transactionId: string;
  amount: string;
  status: string;
  checksumhash: string;
  gatewayResponse: string;
}