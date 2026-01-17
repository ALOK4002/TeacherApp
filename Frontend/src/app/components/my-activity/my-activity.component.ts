import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ActivityService } from '../../services/activity.service';
import { UserActivity, ActivityType } from '../../models/activity.models';

@Component({
  selector: 'app-my-activity',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-activity.component.html',
  styleUrls: ['./my-activity.component.css']
})
export class MyActivityComponent implements OnInit {
  activities: UserActivity[] = [];
  isLoading = true;
  errorMessage = '';
  currentPage = 1;
  pageSize = 20;
  hasMoreActivities = true;

  ActivityType = ActivityType;

  constructor(
    private activityService: ActivityService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadActivities();
  }

  loadActivities(page: number = 1) {
    this.isLoading = true;
    this.activityService.getMyActivities(page, this.pageSize).subscribe({
      next: (activities) => {
        if (page === 1) {
          this.activities = activities;
        } else {
          this.activities = [...this.activities, ...activities];
        }
        
        this.hasMoreActivities = activities.length === this.pageSize;
        this.currentPage = page;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading activities:', error);
        this.errorMessage = 'Failed to load activities';
        this.isLoading = false;
      }
    });
  }

  loadMoreActivities() {
    if (!this.isLoading && this.hasMoreActivities) {
      this.loadActivities(this.currentPage + 1);
    }
  }

  getActivityTypeLabel(activityType: ActivityType): string {
    switch (activityType) {
      case ActivityType.DocumentUploaded: return 'Document Uploaded';
      case ActivityType.DocumentDeleted: return 'Document Deleted';
      case ActivityType.DocumentViewed: return 'Document Viewed';
      case ActivityType.DocumentDownloaded: return 'Document Downloaded';
      case ActivityType.DocumentEmailed: return 'Document Emailed';
      case ActivityType.PaymentInitiated: return 'Payment Initiated';
      case ActivityType.PaymentCompleted: return 'Payment Completed';
      case ActivityType.PaymentFailed: return 'Payment Failed';
      case ActivityType.SubscriptionUpgraded: return 'Subscription Upgraded';
      case ActivityType.SubscriptionExpired: return 'Subscription Expired';
      case ActivityType.ProfileCreated: return 'Profile Created';
      case ActivityType.ProfileUpdated: return 'Profile Updated';
      case ActivityType.Login: return 'Login';
      case ActivityType.Logout: return 'Logout';
      default: return 'Activity';
    }
  }

  getRelativeTime(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (diffInSeconds < 60) {
      return 'Just now';
    } else if (diffInSeconds < 3600) {
      const minutes = Math.floor(diffInSeconds / 60);
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else if (diffInSeconds < 86400) {
      const hours = Math.floor(diffInSeconds / 3600);
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else if (diffInSeconds < 604800) {
      const days = Math.floor(diffInSeconds / 86400);
      return `${days} day${days > 1 ? 's' : ''} ago`;
    } else {
      return date.toLocaleDateString();
    }
  }

  goToDashboard() {
    this.router.navigate(['/user-dashboard']);
  }
}