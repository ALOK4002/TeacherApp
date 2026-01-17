export interface UserActivity {
  id: number;
  activityType: ActivityType;
  activityDescription: string;
  entityType?: string;
  entityId?: number;
  metadata?: string;
  activityDate: string;
  activityIcon: string;
  activityColor: string;
}

export enum ActivityType {
  DocumentUploaded = 0,
  DocumentDeleted = 1,
  DocumentViewed = 2,
  DocumentDownloaded = 3,
  DocumentEmailed = 4,
  PaymentInitiated = 5,
  PaymentCompleted = 6,
  PaymentFailed = 7,
  SubscriptionUpgraded = 8,
  SubscriptionExpired = 9,
  ProfileCreated = 10,
  ProfileUpdated = 11,
  Login = 12,
  Logout = 13
}