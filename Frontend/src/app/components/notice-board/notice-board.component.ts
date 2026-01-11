import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NoticeService } from '../../services/notice.service';
import { AuthService } from '../../services/auth.service';
import { Notice, CreateNotice, NoticeWithReplies } from '../../models/notice.models';

@Component({
  selector: 'app-notice-board',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="notice-board-container">
      <div class="header">
        <h1>Notice Board</h1>
        <div class="header-actions">
          <span class="welcome-text">Welcome, {{ userName }}!</span>
          <button (click)="showAddModal = true" class="btn-add">Post Notice</button>
          <button (click)="showMyNotices = !showMyNotices" class="btn-secondary">
            {{ showMyNotices ? 'All Notices' : 'My Notices' }}
          </button>
          <button (click)="goToAbout()" class="btn-secondary">About Us</button>
          <button (click)="goToTeachers()" class="btn-secondary">Manage Teachers</button>
          <button (click)="goToSchools()" class="btn-secondary">Manage Schools</button>
          <button (click)="logout()" class="btn-logout">Logout</button>
        </div>
      </div>

      <div class="filters">
        <div class="filter-group">
          <label for="categoryFilter">Filter by Category:</label>
          <select id="categoryFilter" [(ngModel)]="selectedCategory" (change)="filterNotices()" class="form-control">
            <option value="">All Categories</option>
            <option value="Information">Information</option>
            <option value="Request">Request</option>
            <option value="Announcement">Announcement</option>
            <option value="General">General</option>
            <option value="Urgent">Urgent</option>
          </select>
        </div>
        <div class="filter-group">
          <label for="priorityFilter">Filter by Priority:</label>
          <select id="priorityFilter" [(ngModel)]="selectedPriority" (change)="filterNotices()" class="form-control">
            <option value="">All Priorities</option>
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Urgent">Urgent</option>
          </select>
        </div>
        <div class="filter-group">
          <label for="searchFilter">Search Notices:</label>
          <input type="text" id="searchFilter" [(ngModel)]="searchTerm" (input)="filterNotices()" 
                 placeholder="Search by title or message..." class="form-control">
        </div>
      </div>

      <div class="notices-container">
        <div class="loading" *ngIf="isLoading">Loading notices...</div>
        <div class="error-message" *ngIf="errorMessage">{{ errorMessage }}</div>
        
        <div class="notices-grid" *ngIf="!isLoading && !errorMessage">
          <div class="notice-card" *ngFor="let notice of filteredNotices" 
               [class.urgent]="notice.priority === 'Urgent'" 
               [class.high]="notice.priority === 'High'">
            <div class="notice-header">
              <div class="notice-title">{{ notice.title }}</div>
              <div class="notice-meta">
                <span class="category" [class]="notice.category.toLowerCase()">{{ notice.category }}</span>
                <span class="priority" [class]="notice.priority.toLowerCase()">{{ notice.priority }}</span>
              </div>
            </div>
            
            <div class="notice-content">
              <p>{{ notice.message }}</p>
            </div>
            
            <div class="notice-footer">
              <div class="notice-info">
                <small>By: {{ notice.postedByUserName }}</small>
                <small>{{ notice.postedDate | date:'short' }}</small>
                <small *ngIf="notice.replyCount > 0">{{ notice.replyCount }} replies</small>
              </div>
              
              <div class="notice-actions">
                <button (click)="replyToNotice(notice)" class="btn-reply" 
                        [disabled]="notice.hasReplied">
                  {{ notice.hasReplied ? 'Replied' : 'Reply' }}
                </button>
                <button *ngIf="isMyNotice(notice)" (click)="editNotice(notice)" class="btn-edit">Edit</button>
                <button *ngIf="isMyNotice(notice)" (click)="viewReplies(notice)" class="btn-view" 
                        [disabled]="notice.replyCount === 0">
                  View Replies ({{ notice.replyCount }})
                </button>
                <button *ngIf="isMyNotice(notice)" (click)="deleteNotice(notice)" class="btn-delete">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Add/Edit Notice Modal -->
      <div class="modal-overlay" *ngIf="showAddModal || showEditModal" (click)="closeNoticeModal()">
        <div class="modal-content" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h2>{{ showAddModal ? 'Post New Notice' : 'Edit Notice' }}</h2>
            <button class="close-btn" (click)="closeNoticeModal()">&times;</button>
          </div>
          <form [formGroup]="noticeForm" (ngSubmit)="saveNotice()" class="notice-form">
            <div class="form-row">
              <div class="form-group">
                <label for="title">Title *</label>
                <input type="text" id="title" formControlName="title" class="form-control" 
                       placeholder="Enter notice title">
                <div class="error-text" *ngIf="noticeForm.get('title')?.invalid && noticeForm.get('title')?.touched">
                  Title is required (max 200 characters)
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="category">Category *</label>
                <select id="category" formControlName="category" class="form-control">
                  <option value="">Select Category</option>
                  <option value="Information">Information</option>
                  <option value="Request">Request</option>
                  <option value="Announcement">Announcement</option>
                  <option value="General">General</option>
                  <option value="Urgent">Urgent</option>
                </select>
                <div class="error-text" *ngIf="noticeForm.get('category')?.invalid && noticeForm.get('category')?.touched">
                  Category selection is required
                </div>
              </div>
              <div class="form-group">
                <label for="priority">Priority *</label>
                <select id="priority" formControlName="priority" class="form-control">
                  <option value="">Select Priority</option>
                  <option value="Low">Low</option>
                  <option value="Medium">Medium</option>
                  <option value="High">High</option>
                  <option value="Urgent">Urgent</option>
                </select>
                <div class="error-text" *ngIf="noticeForm.get('priority')?.invalid && noticeForm.get('priority')?.touched">
                  Priority selection is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group full-width">
                <label for="message">Message *</label>
                <textarea id="message" formControlName="message" class="form-control" rows="5" 
                          placeholder="Enter your notice message..."></textarea>
                <div class="error-text" *ngIf="noticeForm.get('message')?.invalid && noticeForm.get('message')?.touched">
                  Message is required (max 2000 characters)
                </div>
              </div>
            </div>

            <div class="form-row" *ngIf="showEditModal">
              <div class="form-group">
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="isActive"> Active
                </label>
              </div>
            </div>

            <div class="form-actions">
              <button type="button" (click)="closeNoticeModal()" class="btn-cancel">Cancel</button>
              <button type="submit" [disabled]="noticeForm.invalid || isSaving" class="btn-save">
                {{ isSaving ? 'Saving...' : (showAddModal ? 'Post Notice' : 'Save Changes') }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- Reply Modal -->
      <div class="modal-overlay" *ngIf="showReplyModal" (click)="closeReplyModal()">
        <div class="modal-content" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h2>Reply to Notice</h2>
            <button class="close-btn" (click)="closeReplyModal()">&times;</button>
          </div>
          <div class="reply-form">
            <div class="original-notice">
              <h3>{{ selectedNotice?.title }}</h3>
              <p>{{ selectedNotice?.message }}</p>
              <small>By: {{ selectedNotice?.postedByUserName }} on {{ selectedNotice?.postedDate | date:'short' }}</small>
            </div>
            
            <form [formGroup]="replyForm" (ngSubmit)="submitReply()">
              <div class="form-group">
                <label for="replyMessage">Your Reply *</label>
                <textarea id="replyMessage" formControlName="replyMessage" class="form-control" rows="4" 
                          placeholder="Enter your reply..."></textarea>
                <div class="error-text" *ngIf="replyForm.get('replyMessage')?.invalid && replyForm.get('replyMessage')?.touched">
                  Reply message is required (max 1000 characters)
                </div>
              </div>
              
              <div class="form-actions">
                <button type="button" (click)="closeReplyModal()" class="btn-cancel">Cancel</button>
                <button type="submit" [disabled]="replyForm.invalid || isReplying" class="btn-save">
                  {{ isReplying ? 'Sending...' : 'Send Reply' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- View Replies Modal -->
      <div class="modal-overlay" *ngIf="showRepliesModal" (click)="closeRepliesModal()">
        <div class="modal-content large" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h2>Replies to: {{ selectedNoticeWithReplies?.notice?.title }}</h2>
            <button class="close-btn" (click)="closeRepliesModal()">&times;</button>
          </div>
          <div class="replies-content">
            <div class="original-notice">
              <h3>{{ selectedNoticeWithReplies?.notice?.title }}</h3>
              <p>{{ selectedNoticeWithReplies?.notice?.message }}</p>
            </div>
            
            <div class="replies-list">
              <h4>Replies ({{ selectedNoticeWithReplies?.replies?.length || 0 }})</h4>
              <div class="reply-item" *ngFor="let reply of selectedNoticeWithReplies?.replies">
                <div class="reply-header">
                  <strong>{{ reply.repliedByUserName }}</strong>
                  <small>{{ reply.repliedDate | date:'short' }}</small>
                </div>
                <div class="reply-message">{{ reply.replyMessage }}</div>
              </div>
              <div *ngIf="!selectedNoticeWithReplies?.replies?.length" class="no-replies">
                No replies yet.
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .notice-board-container {
      padding: 20px;
      max-width: 1400px;
      margin: 0 auto;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      padding-bottom: 20px;
      border-bottom: 2px solid #e0e0e0;
      flex-wrap: wrap;
      gap: 15px;
    }

    .header h1 {
      color: #2c3e50;
      margin: 0;
      font-size: 2rem;
    }

    .header-actions {
      display: flex;
      align-items: center;
      gap: 10px;
      flex-wrap: wrap;
    }

    .welcome-text {
      color: #34495e;
      font-weight: 500;
      margin-right: 15px;
    }

    .btn-add, .btn-secondary, .btn-logout {
      padding: 8px 16px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      text-decoration: none;
      display: inline-block;
    }

    .btn-add {
      background-color: #28a745;
      color: white;
    }

    .btn-add:hover {
      background-color: #218838;
    }

    .btn-secondary {
      background-color: #007bff;
      color: white;
    }

    .btn-secondary:hover {
      background-color: #0056b3;
    }

    .btn-logout {
      background-color: #e74c3c;
      color: white;
    }

    .btn-logout:hover {
      background-color: #c0392b;
    }

    .filters {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
      padding: 20px;
      background-color: #f8f9fa;
      border-radius: 8px;
      flex-wrap: wrap;
    }

    .filter-group {
      display: flex;
      flex-direction: column;
      gap: 5px;
      min-width: 200px;
    }

    .filter-group label {
      font-weight: 500;
      color: #495057;
    }

    .form-control {
      padding: 8px 12px;
      border: 1px solid #ced4da;
      border-radius: 4px;
      font-size: 14px;
    }

    .form-control:focus {
      outline: none;
      border-color: #007bff;
      box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
    }

    .notices-container {
      margin-top: 20px;
    }

    .loading {
      text-align: center;
      padding: 40px;
      color: #6c757d;
    }

    .error-message {
      text-align: center;
      padding: 20px;
      color: #dc3545;
      background-color: #f8d7da;
      border: 1px solid #f5c6cb;
      border-radius: 4px;
      margin: 20px;
    }

    .notices-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
      gap: 20px;
    }

    .notice-card {
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
      padding: 20px;
      border-left: 4px solid #007bff;
      transition: transform 0.2s, box-shadow 0.2s;
    }

    .notice-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
    }

    .notice-card.urgent {
      border-left-color: #dc3545;
      background-color: #fff5f5;
    }

    .notice-card.high {
      border-left-color: #ffc107;
      background-color: #fffbf0;
    }

    .notice-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 15px;
    }

    .notice-title {
      font-size: 1.2rem;
      font-weight: 600;
      color: #2c3e50;
      flex: 1;
      margin-right: 10px;
    }

    .notice-meta {
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .category, .priority {
      padding: 4px 8px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
      text-align: center;
    }

    .category.information { background-color: #d1ecf1; color: #0c5460; }
    .category.request { background-color: #fff3cd; color: #856404; }
    .category.announcement { background-color: #d4edda; color: #155724; }
    .category.general { background-color: #e2e3e5; color: #383d41; }
    .category.urgent { background-color: #f8d7da; color: #721c24; }

    .priority.low { background-color: #d4edda; color: #155724; }
    .priority.medium { background-color: #fff3cd; color: #856404; }
    .priority.high { background-color: #ffeaa7; color: #b8860b; }
    .priority.urgent { background-color: #f8d7da; color: #721c24; }

    .notice-content {
      margin-bottom: 15px;
    }

    .notice-content p {
      color: #495057;
      line-height: 1.6;
      margin: 0;
    }

    .notice-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-top: 1px solid #e9ecef;
      padding-top: 15px;
    }

    .notice-info {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .notice-info small {
      color: #6c757d;
      font-size: 12px;
    }

    .notice-actions {
      display: flex;
      gap: 8px;
      flex-wrap: wrap;
    }

    .btn-reply, .btn-edit, .btn-view, .btn-delete {
      padding: 6px 12px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 12px;
    }

    .btn-reply {
      background-color: #17a2b8;
      color: white;
    }

    .btn-reply:hover:not(:disabled) {
      background-color: #138496;
    }

    .btn-reply:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .btn-edit {
      background-color: #007bff;
      color: white;
    }

    .btn-edit:hover {
      background-color: #0056b3;
    }

    .btn-view {
      background-color: #28a745;
      color: white;
    }

    .btn-view:hover:not(:disabled) {
      background-color: #218838;
    }

    .btn-view:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .btn-delete {
      background-color: #dc3545;
      color: white;
    }

    .btn-delete:hover {
      background-color: #c82333;
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

    .modal-content {
      background: white;
      border-radius: 8px;
      width: 90%;
      max-width: 600px;
      max-height: 90vh;
      overflow-y: auto;
    }

    .modal-content.large {
      max-width: 800px;
    }

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20px;
      border-bottom: 1px solid #dee2e6;
    }

    .modal-header h2 {
      margin: 0;
      color: #2c3e50;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 24px;
      cursor: pointer;
      color: #6c757d;
    }

    .close-btn:hover {
      color: #495057;
    }

    .notice-form, .reply-form {
      padding: 20px;
    }

    .form-row {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
    }

    .form-group {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .form-group.full-width {
      flex: 2;
    }

    .form-group label {
      font-weight: 500;
      color: #495057;
    }

    .checkbox-label {
      flex-direction: row !important;
      align-items: center;
      gap: 8px !important;
    }

    .error-text {
      color: #dc3545;
      font-size: 12px;
    }

    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 30px;
      padding-top: 20px;
      border-top: 1px solid #dee2e6;
    }

    .btn-cancel, .btn-save {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
    }

    .btn-cancel {
      background-color: #6c757d;
      color: white;
    }

    .btn-cancel:hover {
      background-color: #545b62;
    }

    .btn-save {
      background-color: #28a745;
      color: white;
    }

    .btn-save:hover:not(:disabled) {
      background-color: #218838;
    }

    .btn-save:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .original-notice {
      background-color: #f8f9fa;
      padding: 15px;
      border-radius: 4px;
      margin-bottom: 20px;
    }

    .original-notice h3 {
      margin: 0 0 10px 0;
      color: #2c3e50;
    }

    .original-notice p {
      margin: 0 0 10px 0;
      color: #495057;
    }

    .replies-content {
      padding: 20px;
    }

    .replies-list {
      margin-top: 20px;
    }

    .replies-list h4 {
      color: #2c3e50;
      margin-bottom: 15px;
    }

    .reply-item {
      background-color: #f8f9fa;
      padding: 15px;
      border-radius: 4px;
      margin-bottom: 10px;
    }

    .reply-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 8px;
    }

    .reply-header strong {
      color: #2c3e50;
    }

    .reply-header small {
      color: #6c757d;
    }

    .reply-message {
      color: #495057;
      line-height: 1.5;
    }

    .no-replies {
      text-align: center;
      color: #6c757d;
      font-style: italic;
      padding: 20px;
    }

    @media (max-width: 768px) {
      .notices-grid {
        grid-template-columns: 1fr;
      }
      
      .form-row {
        flex-direction: column;
      }
      
      .filters {
        flex-direction: column;
      }

      .header {
        flex-direction: column;
        align-items: flex-start;
      }

      .header-actions {
        width: 100%;
        justify-content: flex-start;
      }

      .notice-actions {
        flex-direction: column;
        align-items: stretch;
      }

      .notice-actions button {
        margin-bottom: 5px;
      }
    }
  `]
})
export class NoticeBoardComponent implements OnInit {
  notices: Notice[] = [];
  filteredNotices: Notice[] = [];
  selectedCategory: string = '';
  selectedPriority: string = '';
  searchTerm: string = '';
  showMyNotices: boolean = false;
  isLoading = false;
  errorMessage = '';
  userName = '';

  // Modal states
  showAddModal = false;
  showEditModal = false;
  showReplyModal = false;
  showRepliesModal = false;
  
  // Forms
  noticeForm: FormGroup;
  replyForm: FormGroup;
  
  // Current selections
  currentNotice: Notice | null = null;
  selectedNotice: Notice | null = null;
  selectedNoticeWithReplies: NoticeWithReplies | null = null;
  
  // Loading states
  isSaving = false;
  isReplying = false;

  constructor(
    private noticeService: NoticeService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.noticeForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      message: ['', [Validators.required, Validators.maxLength(2000)]],
      category: ['', Validators.required],
      priority: ['', Validators.required],
      isActive: [true]
    });

    this.replyForm = this.fb.group({
      replyMessage: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
    this.loadNotices();
  }

  loadNotices() {
    this.isLoading = true;
    this.errorMessage = '';

    const loadMethod = this.showMyNotices ? 
      this.noticeService.getMyNotices() : 
      this.noticeService.getAllNotices();

    loadMethod.subscribe({
      next: (notices) => {
        this.notices = notices;
        this.filteredNotices = notices;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load notices. Please try again.';
        this.isLoading = false;
        console.error('Error loading notices:', error);
      }
    });
  }

  filterNotices() {
    let filtered = this.notices;

    if (this.selectedCategory) {
      filtered = filtered.filter(notice => notice.category === this.selectedCategory);
    }

    if (this.selectedPriority) {
      filtered = filtered.filter(notice => notice.priority === this.selectedPriority);
    }

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(notice => 
        notice.title.toLowerCase().includes(term) ||
        notice.message.toLowerCase().includes(term)
      );
    }

    this.filteredNotices = filtered;
  }

  isMyNotice(notice: Notice): boolean {
    return notice.postedByUserName === this.userName;
  }

  editNotice(notice: Notice) {
    this.currentNotice = notice;
    this.noticeForm.patchValue({
      title: notice.title,
      message: notice.message,
      category: notice.category,
      priority: notice.priority,
      isActive: notice.isActive
    });
    this.showEditModal = true;
  }

  saveNotice() {
    if (this.noticeForm.valid) {
      this.isSaving = true;
      
      if (this.showAddModal) {
        const createData: CreateNotice = {
          title: this.noticeForm.value.title,
          message: this.noticeForm.value.message,
          category: this.noticeForm.value.category,
          priority: this.noticeForm.value.priority
        };
        
        this.noticeService.createNotice(createData).subscribe({
          next: (newNotice) => {
            this.loadNotices();
            this.closeNoticeModal();
            this.isSaving = false;
          },
          error: (error) => {
            console.error('Error creating notice:', error);
            this.errorMessage = 'Failed to create notice. Please try again.';
            this.isSaving = false;
          }
        });
      } else if (this.showEditModal && this.currentNotice) {
        const updateData = {
          id: this.currentNotice.id,
          ...this.noticeForm.value
        };

        this.noticeService.updateNotice(this.currentNotice.id, updateData).subscribe({
          next: (updatedNotice) => {
            this.loadNotices();
            this.closeNoticeModal();
            this.isSaving = false;
          },
          error: (error) => {
            console.error('Error updating notice:', error);
            this.errorMessage = 'Failed to update notice. Please try again.';
            this.isSaving = false;
          }
        });
      }
    }
  }

  deleteNotice(notice: Notice) {
    if (confirm('Are you sure you want to delete this notice?')) {
      this.noticeService.deleteNotice(notice.id).subscribe({
        next: () => {
          this.loadNotices();
        },
        error: (error) => {
          console.error('Error deleting notice:', error);
          this.errorMessage = 'Failed to delete notice. Please try again.';
        }
      });
    }
  }

  replyToNotice(notice: Notice) {
    this.selectedNotice = notice;
    this.replyForm.reset();
    this.showReplyModal = true;
  }

  submitReply() {
    if (this.replyForm.valid && this.selectedNotice) {
      this.isReplying = true;
      
      const replyData = {
        noticeId: this.selectedNotice.id,
        replyMessage: this.replyForm.value.replyMessage
      };

      this.noticeService.addReply(replyData).subscribe({
        next: () => {
          this.loadNotices(); // Refresh to update reply count and hasReplied status
          this.closeReplyModal();
          this.isReplying = false;
        },
        error: (error) => {
          console.error('Error adding reply:', error);
          this.errorMessage = 'Failed to add reply. Please try again.';
          this.isReplying = false;
        }
      });
    }
  }

  viewReplies(notice: Notice) {
    this.noticeService.getNoticeWithReplies(notice.id).subscribe({
      next: (noticeWithReplies) => {
        this.selectedNoticeWithReplies = noticeWithReplies;
        this.showRepliesModal = true;
      },
      error: (error) => {
        console.error('Error loading replies:', error);
        this.errorMessage = 'Failed to load replies. Please try again.';
      }
    });
  }

  closeNoticeModal() {
    this.showAddModal = false;
    this.showEditModal = false;
    this.currentNotice = null;
    this.noticeForm.reset();
    this.noticeForm.patchValue({ isActive: true });
  }

  closeReplyModal() {
    this.showReplyModal = false;
    this.selectedNotice = null;
    this.replyForm.reset();
  }

  closeRepliesModal() {
    this.showRepliesModal = false;
    this.selectedNoticeWithReplies = null;
  }

  goToTeachers() {
    this.router.navigate(['/teachers']);
  }

  goToSchools() {
    this.router.navigate(['/schools']);
  }

  goToAbout() {
    this.router.navigate(['/about']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}