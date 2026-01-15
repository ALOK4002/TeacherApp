export interface TeacherDocument {
  id: number;
  teacherId: number;
  teacherName: string;
  documentType: string;
  customDocumentType: string;
  fileName: string;
  originalFileName: string;
  blobUrl: string;
  contentType: string;
  fileSizeInBytes: number;
  fileSizeFormatted: string;
  remarks: string;
  uploadedDate: string;
  uploadedByUserId: number;
  isActive: boolean;
}

export interface UploadDocumentRequest {
  teacherId: number;
  file: File;
  documentType: string;
  customDocumentType?: string;
  remarks?: string;
}

export interface SendDocumentEmailRequest {
  documentId: number;
  recipientEmail: string;
  recipientName: string;
  message: string;
}

export interface DocumentSearchRequest {
  teacherId?: number;
  documentType?: string;
  searchTerm?: string;
  fromDate?: string;
  toDate?: string;
  page: number;
  pageSize: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export const DocumentTypes = [
  { value: 'Resume', label: 'Resume/CV' },
  { value: 'Matric', label: 'Matric Certificate (10th)' },
  { value: 'Inter', label: 'Inter Certificate (12th)' },
  { value: 'Graduate', label: 'Graduate Degree' },
  { value: 'PG', label: 'Post Graduate Degree' },
  { value: 'Other', label: 'Other (Custom)' }
];
