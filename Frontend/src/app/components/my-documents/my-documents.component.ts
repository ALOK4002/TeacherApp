import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DocumentService } from '../../services/document.service';
import { SubscriptionService } from '../../services/subscription.service';
import { PaymentService } from '../../services/payment.service';
import { TeacherDocument } from '../../models/document.models';
import { Subscription, SubscriptionTier, CreatePaymentOrder } from '../../models/subscription.models';

@Component({
  selector: 'app-my-documents',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './my-documents.component.html',
  styleUrls: ['./my-documents.component.css']
})
export class MyDocumentsComponent implements OnInit {
  documents: TeacherDocument[] = [];
  subscription: Subscription | null = null;
  uploadForm: FormGroup;
  isLoading = false;
  isUploading = false;
  errorMessage = '';
  successMessage = '';
  selectedFile: File | null = null;
  showEmailDialog = false;
  showUpgradeDialog = false;
  selectedDocumentId: number | null = null;
  emailForm: FormGroup;

  documentTypes = [
    { value: 'Resume', label: 'Resume/CV' },
    { value: 'Matric', label: 'Matric Certificate' },
    { value: 'Inter', label: 'Intermediate Certificate' },
    { value: 'Graduate', label: 'Graduate Degree' },
    { value: 'PG', label: 'Post Graduate Degree' },
    { value: 'Other', label: 'Other Document' }
  ];

  SubscriptionTier = SubscriptionTier;

  constructor(
    private fb: FormBuilder,
    private documentService: DocumentService,
    private subscriptionService: SubscriptionService,
    private paymentService: PaymentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.uploadForm = this.fb.group({
      documentType: ['', [Validators.required]],
      customDocumentType: [''],
      remarks: [''],
      file: [null, [Validators.required]]
    });

    this.emailForm = this.fb.group({
      recipientEmail: ['', [Validators.required, Validators.email]],
      recipientName: ['', [Validators.required]],
      message: ['']
    });
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    let subscriptionLoaded = false;
    let documentsLoaded = false;

    const checkComplete = () => {
      if (subscriptionLoaded && documentsLoaded) {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    };

    // Load subscription
    this.subscriptionService.getMySubscription().subscribe({
      next: (subscription) => {
        this.subscription = subscription;
        subscriptionLoaded = true;
        checkComplete();
      },
      error: (error) => {
        console.error('Error loading subscription:', error);
        subscriptionLoaded = true;
        checkComplete();
      }
    });

    // Load documents
    this.loadDocuments(() => {
      documentsLoaded = true;
      checkComplete();
    });
  }

  loadDocuments(callback?: () => void) {
    this.documentService.getMyDocuments().subscribe({
      next: (documents) => {
        this.documents = documents;
        if (callback) callback();
      },
      error: (error) => {
        console.error('Error loading documents:', error);
        this.errorMessage = 'Failed to load documents';
        if (callback) callback();
      }
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      // Check file size against subscription limits
      if (this.subscription) {
        if (file.size > this.subscription.fileSizeLimitInBytes) {
          this.errorMessage = `File size exceeds limit. Maximum allowed: ${this.subscription.fileSizeLimitFormatted}`;
          event.target.value = '';
          return;
        }
      }

      this.selectedFile = file;
      this.uploadForm.patchValue({ file: file });
      this.errorMessage = '';
    }
  }

  onUpload() {
    if (this.uploadForm.valid && this.selectedFile) {
      // Check subscription limits before upload
      if (this.subscription && this.subscription.remainingUploads <= 0) {
        this.showUpgradeDialog = true;
        return;
      }

      this.isUploading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const { documentType, customDocumentType, remarks } = this.uploadForm.value;

      this.documentService.uploadMyDocument(
        this.selectedFile,
        documentType,
        customDocumentType || '',
        remarks || ''
      ).subscribe({
        next: (response) => {
          this.isUploading = false;
          this.successMessage = 'Document uploaded successfully!';
          this.uploadForm.reset();
          this.selectedFile = null;
          this.loadData(); // Reload both documents and subscription
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.isUploading = false;
          this.errorMessage = error.error?.message || 'Failed to upload document';
          
          // Show upgrade dialog if limit reached
          if (error.error?.message?.includes('Upload limit reached') || 
              error.error?.message?.includes('Upgrade to Premium')) {
            this.showUpgradeDialog = true;
          }
          
          setTimeout(() => this.errorMessage = '', 5000);
        }
      });
    }
  }

  viewDocument(document: TeacherDocument) {
    window.open(document.blobUrl, '_blank');
  }

  downloadDocument(doc: TeacherDocument) {
    this.documentService.downloadDocument(doc.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = window.document.createElement('a');
        link.href = url;
        link.download = doc.originalFileName;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error downloading document:', error);
        this.errorMessage = 'Failed to download document';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  openEmailDialog(documentId: number) {
    this.selectedDocumentId = documentId;
    this.showEmailDialog = true;
    this.emailForm.reset();
  }

  closeEmailDialog() {
    this.showEmailDialog = false;
    this.selectedDocumentId = null;
  }

  sendEmail() {
    if (this.emailForm.valid && this.selectedDocumentId) {
      const emailData = {
        documentId: this.selectedDocumentId,
        ...this.emailForm.value
      };

      this.documentService.sendDocumentByEmail(this.selectedDocumentId, emailData).subscribe({
        next: () => {
          this.successMessage = 'Email sent successfully!';
          this.closeEmailDialog();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = 'Failed to send email';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  deleteDocument(documentId: number) {
    if (confirm('Are you sure you want to delete this document?')) {
      this.documentService.deleteDocument(documentId).subscribe({
        next: () => {
          this.successMessage = 'Document deleted successfully!';
          this.loadData(); // Reload both documents and subscription
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to delete document';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  openUpgradeDialog() {
    this.showUpgradeDialog = true;
  }

  closeUpgradeDialog() {
    this.showUpgradeDialog = false;
  }

  upgradeToPremium() {
    const order: CreatePaymentOrder = {
      subscriptionTier: SubscriptionTier.Premium,
      amount: 99, // ₹99 for premium
      currency: 'INR'
    };

    this.paymentService.createPaymentOrder(order).subscribe({
      next: (response) => {
        this.closeUpgradeDialog();
        // Redirect to Paytm payment gateway
        this.paymentService.initiatePaytmPayment(response);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to create payment order';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getSubscriptionBadgeClass(): string {
    if (!this.subscription) return 'badge-free';
    return this.subscription.tier === SubscriptionTier.Premium ? 'badge-premium' : 'badge-free';
  }

  getSubscriptionLabel(): string {
    if (!this.subscription) return 'Free';
    return this.subscription.tier === SubscriptionTier.Premium ? 'Premium' : 'Free';
  }

  getProgressPercentage(): number {
    if (!this.subscription) return 0;
    return (this.subscription.documentsUploaded / this.subscription.documentUploadLimit) * 100;
  }

  goToDashboard() {
    this.router.navigate(['/user-dashboard']);
  }
}
