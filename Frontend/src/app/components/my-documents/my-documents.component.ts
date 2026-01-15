import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DocumentService } from '../../services/document.service';
import { TeacherDocument } from '../../models/document.models';

@Component({
  selector: 'app-my-documents',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './my-documents.component.html',
  styleUrls: ['./my-documents.component.css']
})
export class MyDocumentsComponent implements OnInit {
  documents: TeacherDocument[] = [];
  uploadForm: FormGroup;
  isLoading = false;
  isUploading = false;
  errorMessage = '';
  successMessage = '';
  selectedFile: File | null = null;
  showEmailDialog = false;
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

  constructor(
    private fb: FormBuilder,
    private documentService: DocumentService,
    private router: Router
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
    this.loadDocuments();
  }

  loadDocuments() {
    this.isLoading = true;
    this.documentService.getMyDocuments().subscribe({
      next: (documents) => {
        this.documents = documents;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading documents:', error);
        this.errorMessage = 'Failed to load documents';
        this.isLoading = false;
      }
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.uploadForm.patchValue({ file: file });
    }
  }

  onUpload() {
    if (this.uploadForm.valid && this.selectedFile) {
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
          this.loadDocuments();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.isUploading = false;
          this.errorMessage = error.error?.message || 'Failed to upload document';
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
          this.loadDocuments();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to delete document';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  goToDashboard() {
    this.router.navigate(['/user-dashboard']);
  }
}
