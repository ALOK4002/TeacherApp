import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentService } from '../../services/document.service';
import { TeacherService } from '../../services/teacher.service';
import { AuthService } from '../../services/auth.service';
import { TeacherDocument, DocumentTypes, SendDocumentEmailRequest } from '../../models/document.models';
import { Teacher } from '../../models/teacher.models';

@Component({
  selector: 'app-teacher-documents',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="documents-container">
      <div class="header-section">
        <div class="header-content">
          <button (click)="goBack()" class="btn-back">
            <span class="icon">←</span> Back to Teachers
          </button>
          <h1>📄 Teacher Documents</h1>
          <div class="teacher-info" *ngIf="teacher">
            <h2>{{ teacher.teacherName }}</h2>
            <p>{{ teacher.email }} | {{ teacher.contactNumber }}</p>
            <p>{{ teacher.schoolName }} - {{ teacher.district }}</p>
          </div>
        </div>
      </div>

      <!-- Upload Section -->
      <div class="upload-section card">
        <h3>📤 Upload New Document</h3>
        <div class="upload-form">
          <div class="form-row">
            <div class="form-group">
              <label>Document Type *</label>
              <select [(ngModel)]="uploadData.documentType" class="form-control">
                <option value="">Select Type</option>
                <option *ngFor="let type of documentTypes" [value]="type.value">
                  {{ type.label }}
                </option>
              </select>
            </div>

            <div class="form-group" *ngIf="uploadData.documentType === 'Other'">
              <label>Custom Type *</label>
              <input type="text" [(ngModel)]="uploadData.customDocumentType" 
                     class="form-control" placeholder="Enter document type">
            </div>
          </div>

          <div class="form-group">
            <label>Remarks</label>
            <textarea [(ngModel)]="uploadData.remarks" class="form-control" 
                      rows="2" placeholder="Add any notes or remarks..."></textarea>
          </div>

          <div class="form-group">
            <label>Select File *</label>
            <input type="file" (change)="onFileSelected($event)" 
                   class="form-control" accept=".pdf,.doc,.docx,.jpg,.jpeg,.png">
            <small *ngIf="selectedFile">Selected: {{ selectedFile.name }} ({{ formatFileSize(selectedFile.size) }})</small>
          </div>

          <div class="form-actions">
            <button (click)="uploadDocument()" [disabled]="!canUpload() || uploading" 
                    class="btn-primary">
              <span *ngIf="!uploading">📤 Upload Document</span>
              <span *ngIf="uploading">⏳ Uploading...</span>
            </button>
            <button (click)="resetUploadForm()" class="btn-secondary">Clear</button>
          </div>

          <div *ngIf="uploadError" class="alert alert-error">{{ uploadError }}</div>
          <div *ngIf="uploadSuccess" class="alert alert-success">{{ uploadSuccess }}</div>
        </div>
      </div>

      <!-- Documents List -->
      <div class="documents-list card">
        <h3>📋 Uploaded Documents ({{ documents.length }})</h3>
        
        <!-- Global Messages -->
        <div *ngIf="uploadError" class="alert alert-error">{{ uploadError }}</div>
        <div *ngIf="uploadSuccess" class="alert alert-success">{{ uploadSuccess }}</div>
        
        <div *ngIf="loading" class="loading">Loading documents...</div>
        
        <div *ngIf="!loading && documents.length === 0" class="no-documents">
          <p>📭 No documents uploaded yet</p>
          <p>Upload your first document using the form above</p>
        </div>

        <div *ngIf="!loading && documents.length > 0" class="documents-grid">
          <div *ngFor="let doc of documents" class="document-card">
            <div class="document-header">
              <div class="document-icon">📄</div>
              <div class="document-info">
                <h4>{{ doc.originalFileName }}</h4>
                <span class="document-type">{{ getDocumentTypeLabel(doc.documentType) }}</span>
                <span *ngIf="doc.customDocumentType" class="custom-type">{{ doc.customDocumentType }}</span>
              </div>
            </div>

            <div class="document-details">
              <p><strong>Size:</strong> {{ doc.fileSizeFormatted }}</p>
              <p><strong>Uploaded:</strong> {{ doc.uploadedDate | date:'medium' }}</p>
              <p *ngIf="doc.remarks"><strong>Remarks:</strong> {{ doc.remarks }}</p>
            </div>

            <div class="document-actions">
              <button (click)="viewDocument(doc)" class="btn-action btn-view" title="View">
                👁️ View
              </button>
              <button (click)="downloadDocument(doc)" class="btn-action btn-download" title="Download">
                ⬇️ Download
              </button>
              <button (click)="openEmailDialog(doc)" class="btn-action btn-email" title="Send Email">
                ✉️ Email
              </button>
              <button (click)="deleteDocument(doc)" class="btn-action btn-delete" title="Delete">
                🗑️ Delete
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Email Dialog -->
      <div *ngIf="showEmailDialog" class="modal-overlay" (click)="closeEmailDialog()">
        <div class="modal-content" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h3>✉️ Send Document via Email</h3>
            <button (click)="closeEmailDialog()" class="btn-close">×</button>
          </div>
          <div class="modal-body">
            <p><strong>Document:</strong> {{ selectedDocument?.originalFileName }}</p>
            
            <div class="form-group">
              <label>Recipient Name *</label>
              <input type="text" [(ngModel)]="emailData.recipientName" 
                     class="form-control" placeholder="Enter recipient name">
            </div>

            <div class="form-group">
              <label>Recipient Email *</label>
              <input type="email" [(ngModel)]="emailData.recipientEmail" 
                     class="form-control" placeholder="Enter email address">
            </div>

            <div class="form-group">
              <label>Message</label>
              <textarea [(ngModel)]="emailData.message" class="form-control" 
                        rows="4" placeholder="Add a message..."></textarea>
            </div>

            <div *ngIf="emailError" class="alert alert-error">{{ emailError }}</div>
            <div *ngIf="emailSuccess" class="alert alert-success">{{ emailSuccess }}</div>
          </div>
          <div class="modal-footer">
            <button (click)="sendEmail()" [disabled]="!canSendEmail() || sendingEmail" 
                    class="btn-primary">
              <span *ngIf="!sendingEmail">📧 Send Email</span>
              <span *ngIf="sendingEmail">⏳ Sending...</span>
            </button>
            <button (click)="closeEmailDialog()" class="btn-secondary">Cancel</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .documents-container {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    .header-section {
      background: linear-gradient(135deg, #0078d4 0%, #8764b8 100%);
      color: white;
      padding: 30px;
      border-radius: 8px;
      margin-bottom: 20px;
    }

    .header-content h1 {
      margin: 10px 0;
      font-size: 28px;
    }

    .teacher-info {
      margin-top: 15px;
      padding-top: 15px;
      border-top: 1px solid rgba(255,255,255,0.3);
    }

    .teacher-info h2 {
      margin: 0 0 5px 0;
      font-size: 20px;
    }

    .teacher-info p {
      margin: 3px 0;
      opacity: 0.9;
    }

    .btn-back {
      background: rgba(255,255,255,0.2);
      color: white;
      border: 1px solid rgba(255,255,255,0.3);
      padding: 8px 16px;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      margin-bottom: 10px;
    }

    .btn-back:hover {
      background: rgba(255,255,255,0.3);
    }

    .card {
      background: white;
      border-radius: 8px;
      padding: 25px;
      margin-bottom: 20px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }

    .card h3 {
      margin: 0 0 20px 0;
      color: #333;
      font-size: 20px;
    }

    .form-row {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 15px;
      margin-bottom: 15px;
    }

    .form-group {
      margin-bottom: 15px;
    }

    .form-group label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
      color: #555;
    }

    .form-control {
      width: 100%;
      padding: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    .form-control:focus {
      outline: none;
      border-color: #0078d4;
    }

    textarea.form-control {
      resize: vertical;
    }

    .form-group small {
      display: block;
      margin-top: 5px;
      color: #666;
      font-size: 12px;
    }

    .form-actions {
      display: flex;
      gap: 10px;
      margin-top: 20px;
    }

    .btn-primary, .btn-secondary, .btn-action {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      font-weight: 500;
      transition: all 0.2s;
    }

    .btn-primary {
      background: #0078d4;
      color: white;
    }

    .btn-primary:hover:not(:disabled) {
      background: #106ebe;
    }

    .btn-primary:disabled {
      background: #ccc;
      cursor: not-allowed;
    }

    .btn-secondary {
      background: #f3f2f1;
      color: #333;
    }

    .btn-secondary:hover {
      background: #e1dfdd;
    }

    .alert {
      padding: 15px 20px;
      border-radius: 6px;
      margin-top: 15px;
      font-weight: 500;
      display: flex;
      align-items: center;
      gap: 10px;
      animation: slideIn 0.3s ease-out;
    }

    @keyframes slideIn {
      from {
        opacity: 0;
        transform: translateY(-10px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    .alert-error {
      background: #fde7e9;
      color: #d13438;
      border: 2px solid #d13438;
    }

    .alert-error::before {
      content: '❌';
      font-size: 18px;
    }

    .alert-success {
      background: #dff6dd;
      color: #107c10;
      border: 2px solid #107c10;
    }

    .alert-success::before {
      content: '✅';
      font-size: 18px;
    }

    .loading, .no-documents {
      text-align: center;
      padding: 40px;
      color: #666;
    }

    .no-documents p {
      margin: 10px 0;
    }

    .documents-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 20px;
    }

    .document-card {
      border: 1px solid #e1dfdd;
      border-radius: 8px;
      padding: 15px;
      transition: all 0.2s;
    }

    .document-card:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      transform: translateY(-2px);
    }

    .document-header {
      display: flex;
      gap: 12px;
      margin-bottom: 12px;
    }

    .document-icon {
      font-size: 32px;
    }

    .document-info h4 {
      margin: 0 0 5px 0;
      font-size: 14px;
      color: #333;
      word-break: break-word;
    }

    .document-type {
      display: inline-block;
      background: #0078d4;
      color: white;
      padding: 2px 8px;
      border-radius: 3px;
      font-size: 11px;
      margin-right: 5px;
    }

    .custom-type {
      display: inline-block;
      background: #8764b8;
      color: white;
      padding: 2px 8px;
      border-radius: 3px;
      font-size: 11px;
    }

    .document-details {
      margin: 12px 0;
      font-size: 13px;
      color: #666;
    }

    .document-details p {
      margin: 5px 0;
    }

    .document-actions {
      display: flex;
      gap: 8px;
      flex-wrap: wrap;
    }

    .btn-action {
      flex: 1;
      min-width: 70px;
      padding: 8px 12px;
      font-size: 12px;
    }

    .btn-view {
      background: #00bcf2;
      color: white;
    }

    .btn-view:hover {
      background: #00a0d6;
    }

    .btn-download {
      background: #107c10;
      color: white;
    }

    .btn-download:hover {
      background: #0e6b0e;
    }

    .btn-email {
      background: #ff8c00;
      color: white;
    }

    .btn-email:hover {
      background: #e67e00;
    }

    .btn-delete {
      background: #d13438;
      color: white;
    }

    .btn-delete:hover {
      background: #b71c1c;
    }

    .modal-overlay {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(0,0,0,0.5);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 1000;
    }

    .modal-content {
      background: white;
      border-radius: 8px;
      width: 90%;
      max-width: 500px;
      max-height: 90vh;
      overflow-y: auto;
    }

    .modal-header {
      padding: 20px;
      border-bottom: 1px solid #e1dfdd;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .modal-header h3 {
      margin: 0;
      font-size: 18px;
    }

    .btn-close {
      background: none;
      border: none;
      font-size: 28px;
      cursor: pointer;
      color: #666;
      padding: 0;
      width: 30px;
      height: 30px;
      line-height: 1;
    }

    .btn-close:hover {
      color: #333;
    }

    .modal-body {
      padding: 20px;
    }

    .modal-footer {
      padding: 20px;
      border-top: 1px solid #e1dfdd;
      display: flex;
      gap: 10px;
      justify-content: flex-end;
    }

    @media (max-width: 768px) {
      .documents-container {
        padding: 10px;
      }

      .form-row {
        grid-template-columns: 1fr;
      }

      .documents-grid {
        grid-template-columns: 1fr;
      }

      .document-actions {
        flex-direction: column;
      }

      .btn-action {
        width: 100%;
      }
    }
  `]
})
export class TeacherDocumentsComponent implements OnInit {
  teacherId!: number;
  teacher: Teacher | null = null;
  documents: TeacherDocument[] = [];
  documentTypes = DocumentTypes;
  loading = false;
  uploading = false;
  uploadError = '';
  uploadSuccess = '';
  
  selectedFile: File | null = null;
  uploadData = {
    documentType: '',
    customDocumentType: '',
    remarks: ''
  };

  showEmailDialog = false;
  selectedDocument: TeacherDocument | null = null;
  sendingEmail = false;
  emailError = '';
  emailSuccess = '';
  emailData = {
    recipientName: '',
    recipientEmail: '',
    message: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private documentService: DocumentService,
    private teacherService: TeacherService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.teacherId = +params['id'];
      this.loadTeacher();
      this.loadDocuments();
    });
  }

  loadTeacher(): void {
    this.teacherService.getTeacherById(this.teacherId).subscribe({
      next: (teacher) => {
        this.teacher = teacher;
      },
      error: (error) => {
        console.error('Error loading teacher:', error);
      }
    });
  }

  loadDocuments(): void {
    this.loading = true;
    this.documentService.getDocumentsByTeacher(this.teacherId).subscribe({
      next: (documents) => {
        this.documents = documents;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading documents:', error);
        this.loading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  canUpload(): boolean {
    return !!(this.selectedFile && this.uploadData.documentType &&
      (this.uploadData.documentType !== 'Other' || this.uploadData.customDocumentType));
  }

  uploadDocument(): void {
    if (!this.canUpload() || !this.selectedFile) return;

    this.uploading = true;
    this.uploadError = '';
    this.uploadSuccess = '';

    this.documentService.uploadDocument(
      this.teacherId,
      this.selectedFile,
      this.uploadData.documentType,
      this.uploadData.customDocumentType,
      this.uploadData.remarks
    ).subscribe({
      next: (document) => {
        this.uploading = false;
        this.uploadSuccess = `✅ Document "${document.originalFileName}" uploaded successfully!`;
        this.resetUploadForm();
        this.loadDocuments();
        // Clear success message after 5 seconds
        setTimeout(() => this.uploadSuccess = '', 5000);
      },
      error: (error) => {
        this.uploading = false;
        console.error('Upload error:', error);
        this.uploadError = error.error?.message || error.message || 'Failed to upload document. Please try again.';
        // Clear error message after 10 seconds
        setTimeout(() => this.uploadError = '', 10000);
      }
    });
  }

  resetUploadForm(): void {
    this.selectedFile = null;
    this.uploadData = {
      documentType: '',
      customDocumentType: '',
      remarks: ''
    };
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) fileInput.value = '';
  }

  viewDocument(doc: TeacherDocument): void {
    window.open(doc.blobUrl, '_blank');
  }

  downloadDocument(doc: TeacherDocument): void {
    this.documentService.downloadDocument(doc.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = doc.originalFileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error downloading document:', error);
        this.uploadError = 'Failed to download document. Please try again.';
        setTimeout(() => this.uploadError = '', 5000);
      }
    });
  }

  deleteDocument(doc: TeacherDocument): void {
    if (!confirm(`Are you sure you want to delete "${doc.originalFileName}"?`)) {
      return;
    }

    this.documentService.deleteDocument(doc.id).subscribe({
      next: () => {
        this.uploadSuccess = `✅ Document "${doc.originalFileName}" deleted successfully!`;
        this.loadDocuments();
        setTimeout(() => this.uploadSuccess = '', 5000);
      },
      error: (error) => {
        console.error('Error deleting document:', error);
        this.uploadError = 'Failed to delete document. Please try again.';
        setTimeout(() => this.uploadError = '', 5000);
      }
    });
  }

  openEmailDialog(doc: TeacherDocument): void {
    this.selectedDocument = doc;
    this.showEmailDialog = true;
    this.emailError = '';
    this.emailSuccess = '';
    this.emailData = {
      recipientName: '',
      recipientEmail: '',
      message: `Please find attached the document: ${doc.originalFileName}`
    };
  }

  closeEmailDialog(): void {
    this.showEmailDialog = false;
    this.selectedDocument = null;
  }

  canSendEmail(): boolean {
    return !!(this.emailData.recipientName && this.emailData.recipientEmail);
  }

  sendEmail(): void {
    if (!this.canSendEmail() || !this.selectedDocument) return;

    this.sendingEmail = true;
    this.emailError = '';
    this.emailSuccess = '';

    const request: SendDocumentEmailRequest = {
      documentId: this.selectedDocument.id,
      recipientEmail: this.emailData.recipientEmail,
      recipientName: this.emailData.recipientName,
      message: this.emailData.message
    };

    this.documentService.sendDocumentByEmail(this.selectedDocument.id, request).subscribe({
      next: () => {
        this.sendingEmail = false;
        this.emailSuccess = 'Email sent successfully!';
        setTimeout(() => this.closeEmailDialog(), 2000);
      },
      error: (error) => {
        this.sendingEmail = false;
        this.emailError = error.error?.message || 'Failed to send email. Please try again.';
      }
    });
  }

  getDocumentTypeLabel(type: string): string {
    const docType = this.documentTypes.find(t => t.value === type);
    return docType ? docType.label : type;
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }

  goBack(): void {
    this.router.navigate(['/teachers']);
  }
}
