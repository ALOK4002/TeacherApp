import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NoticeService } from '../../services/notice.service';
import { Notice, CreateNoticeReply } from '../../models/notice.models';

@Component({
  selector: 'app-notice-widget',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="notice-widget">
      <div class="widget-header">
        <h3>📢 Notice Board</h3>
        <small>Latest notices and announcements</small>
      </div>
      
      <div class="notices-list" *ngIf="notices.length > 0">
        <div class="notice-item" *ngFor="let notice of notices" 
             [class.urgent]="notice.priority === 'Urgent'" 
             [class.high]="notice.priority === 'High'">
          <div class="notice-header">
            <div class="notice-title">{{ notice.title }}</div>
            <div class="notice-badges">
              <span class="category-badge" [class]="notice.category.toLowerCase()">{{ notice.category }}</span>
              <span class="priority-badge" [class]="notice.priority.toLowerCase()">{{ notice.priority }}</span>
            </div>
          </div>
          
          <div class="notice-content">
            <p>{{ notice.message }}</p>
          </div>
          
          <div class="notice-footer">
            <div class="notice-meta">
              <small>By: {{ notice.postedByUserName }}</small>
              <small>{{ notice.postedDate | date:'short' }}</small>
              <small *ngIf="notice.replyCount > 0">{{ notice.replyCount }} replies</small>
            </div>
            
            <div class="notice-actions">
              <button (click)="replyToNotice(notice)" class="btn-reply-small" 
                      [disabled]="notice.hasReplied">
                {{ notice.hasReplied ? '✓ Replied' : 'Reply' }}
              </button>
            </div>
          </div>
        </div>
      </div>
      
      <div class="no-notices" *ngIf="notices.length === 0 && !isLoading">
        <p>No notices available at the moment.</p>
      </div>
      
      <div class="loading-widget" *ngIf="isLoading">
        <p>Loading notices...</p>
      </div>

      <!-- Quick Reply Modal -->
      <div class="modal-overlay" *ngIf="showReplyModal" (click)="closeReplyModal()">
        <div class="modal-content-small" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h4>Quick Reply</h4>
            <button class="close-btn" (click)="closeReplyModal()">&times;</button>
          </div>
          
          <div class="quick-reply-content">
            <div class="original-notice-small">
              <strong>{{ selectedNotice?.title }}</strong>
              <p>{{ selectedNotice?.message }}</p>
            </div>
            
            <form [formGroup]="replyForm" (ngSubmit)="submitReply()">
              <div class="form-group">
                <label for="quickReply">Your Reply:</label>
                <textarea id="quickReply" formControlName="replyMessage" 
                          class="form-control-small" rows="3" 
                          placeholder="Enter your reply..."></textarea>
                <div class="error-text" *ngIf="replyForm.get('replyMessage')?.invalid && replyForm.get('replyMessage')?.touched">
                  Reply is required (max 1000 characters)
                </div>
              </div>
              
              <div class="form-actions-small">
                <button type="button" (click)="closeReplyModal()" class="btn-cancel-small">Cancel</button>
                <button type="submit" [disabled]="replyForm.invalid || isReplying" class="btn-send-small">
                  {{ isReplying ? 'Sending...' : 'Send' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .notice-widget {
      background: #f8f9fa;
      border-radius: 8px;
      padding: 20px;
      margin-top: 30px;
      border: 1px solid #e9ecef;
      max-height: 500px;
      overflow-y: auto;
    }

    .widget-header {
      text-align: center;
      margin-bottom: 20px;
      padding-bottom: 15px;
      border-bottom: 2px solid #dee2e6;
    }

    .widget-header h3 {
      margin: 0 0 5px 0;
      color: #2c3e50;
      font-size: 1.3rem;
    }

    .widget-header small {
      color: #6c757d;
      font-style: italic;
    }

    .notices-list {
      display: flex;
      flex-direction: column;
      gap: 15px;
    }

    .notice-item {
      background: white;
      border-radius: 6px;
      padding: 15px;
      border-left: 3px solid #007bff;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      transition: transform 0.2s;
    }

    .notice-item:hover {
      transform: translateY(-1px);
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
    }

    .notice-item.urgent {
      border-left-color: #dc3545;
      background-color: #fff5f5;
    }

    .notice-item.high {
      border-left-color: #ffc107;
      background-color: #fffbf0;
    }

    .notice-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 10px;
    }

    .notice-title {
      font-weight: 600;
      color: #2c3e50;
      font-size: 1rem;
      flex: 1;
      margin-right: 10px;
    }

    .notice-badges {
      display: flex;
      flex-direction: column;
      gap: 3px;
    }

    .category-badge, .priority-badge {
      padding: 2px 6px;
      border-radius: 8px;
      font-size: 10px;
      font-weight: 500;
      text-align: center;
    }

    .category-badge.information { background-color: #d1ecf1; color: #0c5460; }
    .category-badge.request { background-color: #fff3cd; color: #856404; }
    .category-badge.announcement { background-color: #d4edda; color: #155724; }
    .category-badge.general { background-color: #e2e3e5; color: #383d41; }
    .category-badge.urgent { background-color: #f8d7da; color: #721c24; }

    .priority-badge.low { background-color: #d4edda; color: #155724; }
    .priority-badge.medium { background-color: #fff3cd; color: #856404; }
    .priority-badge.high { background-color: #ffeaa7; color: #b8860b; }
    .priority-badge.urgent { background-color: #f8d7da; color: #721c24; }

    .notice-content {
      margin-bottom: 10px;
    }

    .notice-content p {
      color: #495057;
      font-size: 14px;
      line-height: 1.4;
      margin: 0;
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    .notice-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-top: 1px solid #e9ecef;
      padding-top: 8px;
    }

    .notice-meta {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .notice-meta small {
      color: #6c757d;
      font-size: 11px;
    }

    .notice-actions {
      display: flex;
      gap: 5px;
    }

    .btn-reply-small {
      padding: 4px 8px;
      background-color: #17a2b8;
      color: white;
      border: none;
      border-radius: 3px;
      cursor: pointer;
      font-size: 11px;
    }

    .btn-reply-small:hover:not(:disabled) {
      background-color: #138496;
    }

    .btn-reply-small:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .no-notices {
      text-align: center;
      color: #6c757d;
      font-style: italic;
      padding: 20px;
    }

    .loading-widget {
      text-align: center;
      color: #6c757d;
      padding: 20px;
    }

    /* Modal Styles */
    .modal-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.5);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }

    .modal-content-small {
      background: white;
      border-radius: 8px;
      width: 90%;
      max-width: 500px;
      max-height: 80vh;
      overflow-y: auto;
    }

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px;
      border-bottom: 1px solid #dee2e6;
    }

    .modal-header h4 {
      margin: 0;
      color: #2c3e50;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 20px;
      cursor: pointer;
      color: #6c757d;
    }

    .close-btn:hover {
      color: #495057;
    }

    .quick-reply-content {
      padding: 15px;
    }

    .original-notice-small {
      background-color: #f8f9fa;
      padding: 10px;
      border-radius: 4px;
      margin-bottom: 15px;
    }

    .original-notice-small strong {
      color: #2c3e50;
      display: block;
      margin-bottom: 5px;
    }

    .original-notice-small p {
      margin: 0;
      color: #495057;
      font-size: 13px;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    .form-group {
      margin-bottom: 15px;
    }

    .form-group label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
      color: #495057;
      font-size: 14px;
    }

    .form-control-small {
      width: 100%;
      padding: 8px;
      border: 1px solid #ced4da;
      border-radius: 4px;
      font-size: 13px;
      resize: vertical;
      box-sizing: border-box;
    }

    .form-control-small:focus {
      outline: none;
      border-color: #007bff;
      box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
    }

    .error-text {
      color: #dc3545;
      font-size: 11px;
      margin-top: 3px;
    }

    .form-actions-small {
      display: flex;
      justify-content: flex-end;
      gap: 8px;
      margin-top: 15px;
    }

    .btn-cancel-small, .btn-send-small {
      padding: 6px 12px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 12px;
    }

    .btn-cancel-small {
      background-color: #6c757d;
      color: white;
    }

    .btn-cancel-small:hover {
      background-color: #545b62;
    }

    .btn-send-small {
      background-color: #28a745;
      color: white;
    }

    .btn-send-small:hover:not(:disabled) {
      background-color: #218838;
    }

    .btn-send-small:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    @media (max-width: 768px) {
      .notice-widget {
        margin-top: 20px;
        padding: 15px;
      }

      .notice-header {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
      }

      .notice-badges {
        flex-direction: row;
        gap: 5px;
      }

      .notice-footer {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
      }
    }
  `]
})
export class NoticeWidgetComponent implements OnInit {
  notices: Notice[] = [];
  isLoading = false;
  showReplyModal = false;
  selectedNotice: Notice | null = null;
  replyForm: FormGroup;
  isReplying = false;

  constructor(
    private noticeService: NoticeService,
    private fb: FormBuilder
  ) {
    this.replyForm = this.fb.group({
      replyMessage: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  ngOnInit() {
    this.loadNotices();
  }

  loadNotices() {
    this.isLoading = true;
    
    this.noticeService.getAllNotices().subscribe({
      next: (notices) => {
        // Show only the latest 5 notices, prioritizing urgent and high priority
        this.notices = notices
          .sort((a, b) => {
            // First sort by priority (Urgent > High > Medium > Low)
            const priorityOrder = { 'Urgent': 4, 'High': 3, 'Medium': 2, 'Low': 1 };
            const priorityDiff = (priorityOrder[b.priority as keyof typeof priorityOrder] || 0) - 
                                (priorityOrder[a.priority as keyof typeof priorityOrder] || 0);
            if (priorityDiff !== 0) return priorityDiff;
            
            // Then sort by date (newest first)
            return new Date(b.postedDate).getTime() - new Date(a.postedDate).getTime();
          })
          .slice(0, 5);
        
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading notices:', error);
        this.isLoading = false;
      }
    });
  }

  replyToNotice(notice: Notice) {
    this.selectedNotice = notice;
    this.replyForm.reset();
    this.showReplyModal = true;
  }

  submitReply() {
    if (this.replyForm.valid && this.selectedNotice) {
      this.isReplying = true;
      
      const replyData: CreateNoticeReply = {
        noticeId: this.selectedNotice.id,
        replyMessage: this.replyForm.value.replyMessage
      };

      this.noticeService.addReply(replyData).subscribe({
        next: () => {
          this.loadNotices(); // Refresh to update reply status
          this.closeReplyModal();
          this.isReplying = false;
        },
        error: (error) => {
          console.error('Error adding reply:', error);
          this.isReplying = false;
        }
      });
    }
  }

  closeReplyModal() {
    this.showReplyModal = false;
    this.selectedNotice = null;
    this.replyForm.reset();
  }
}