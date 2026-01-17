export interface Subscription {
  id: number;
  userId: number;
  tier: SubscriptionTier;
  status: SubscriptionStatus;
  startDate: string;
  endDate?: string;
  documentUploadLimit: number;
  fileSizeLimitInBytes: number;
  fileSizeLimitFormatted: string;
  documentsUploaded: number;
  remainingUploads: number;
  isActive: boolean;
  createdDate: string;
}

export enum SubscriptionTier {
  Free = 0,
  Premium = 1
}

export enum SubscriptionStatus {
  Active = 0,
  Expired = 1,
  Cancelled = 2,
  PendingApproval = 3
}

export interface UploadEligibility {
  canUpload: boolean;
  documentsUploaded: number;
  documentUploadLimit: number;
  remainingUploads: number;
  fileSizeLimitInBytes: number;
  fileSizeLimitFormatted: string;
  tier: SubscriptionTier;
}

export interface CreatePaymentOrder {
  subscriptionTier: SubscriptionTier;
  amount: number;
  currency: string;
}

export interface PaymentOrderResponse {
  orderId: string;
  amount: number;
  currency: string;
  merchantId: string;
  checksumHash: string;
  callbackUrl: string;
  paytmUrl: string;
}