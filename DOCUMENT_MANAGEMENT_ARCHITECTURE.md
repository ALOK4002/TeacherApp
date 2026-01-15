# Document Management System Architecture

## System Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                         USER INTERFACE                               │
│                    (Angular Frontend - Port 4200)                    │
│                                                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │
│  │   Teacher    │  │   Document   │  │    Email     │              │
│  │  Management  │  │    Upload    │  │    Dialog    │              │
│  └──────────────┘  └──────────────┘  └──────────────┘              │
└───────────────────────────────┬─────────────────────────────────────┘
                                │
                                │ HTTP/REST API
                                │ (JWT Authentication)
                                ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      BACKEND API                                     │
│                 (ASP.NET Core - Port 5162)                          │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    Controllers Layer                          │  │
│  │  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐ │  │
│  │  │     Auth       │  │    Teacher     │  │  TeacherDoc    │ │  │
│  │  │  Controller    │  │   Controller   │  │  Controller    │ │  │
│  │  └────────────────┘  └────────────────┘  └────────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                │                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    Services Layer                             │  │
│  │  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐ │  │
│  │  │  TeacherDoc    │  │   Document     │  │     Email      │ │  │
│  │  │    Service     │  │    Storage     │  │    Service     │ │  │
│  │  └────────────────┘  └────────────────┘  └────────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                │                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                  Repository Layer                             │  │
│  │  ┌────────────────┐  ┌────────────────┐                      │  │
│  │  │    Teacher     │  │  TeacherDoc    │                      │  │
│  │  │   Repository   │  │   Repository   │                      │  │
│  │  └────────────────┘  └────────────────┘                      │  │
│  └──────────────────────────────────────────────────────────────┘  │
└───────────────────────┬───────────────────┬─────────────────────────┘
                        │                   │
                        │                   │
        ┌───────────────▼──────┐   ┌────────▼────────────┐
        │                      │   │                     │
        │   SQLite Database    │   │   Azure Services    │
        │   (teacherportal.db) │   │                     │
        │                      │   │  ┌───────────────┐  │
        │  ┌────────────────┐  │   │  │ Blob Storage  │  │
        │  │    Teachers    │  │   │  │  Container:   │  │
        │  │     Table      │  │   │  │   teacher-    │  │
        │  └────────────────┘  │   │  │  documents    │  │
        │                      │   │  └───────────────┘  │
        │  ┌────────────────┐  │   │                     │
        │  │ TeacherDocs    │  │   │  ┌───────────────┐  │
        │  │     Table      │  │   │  │ Communication │  │
        │  └────────────────┘  │   │  │   Services    │  │
        │                      │   │  │   (Email)     │  │
        └──────────────────────┘   │  └───────────────┘  │
                                   │                     │
                                   └─────────────────────┘
```

## Data Flow Diagrams

### 1. Document Upload Flow

```
┌──────────┐
│  User    │
└────┬─────┘
     │ 1. Select file & document type
     ▼
┌──────────────────┐
│  Upload Form     │
│  (Frontend)      │
└────┬─────────────┘
     │ 2. POST /api/teacherdocument/upload
     │    (multipart/form-data)
     ▼
┌──────────────────────────┐
│ TeacherDocumentController│
│ (Backend)                │
└────┬─────────────────────┘
     │ 3. Validate file & user
     ▼
┌──────────────────────────┐
│ TeacherDocumentService   │
└────┬─────────────────────┘
     │ 4. Generate unique filename
     ▼
┌──────────────────────────┐
│ DocumentStorageService   │
│ (Azure Blob)             │
└────┬─────────────────────┘
     │ 5. Upload to Azure Blob Storage
     │    Container: teacher-documents
     │    Path: {teacherId}/{docType}/{guid}.ext
     ▼
┌──────────────────────────┐
│ Azure Blob Storage       │
│ ✓ File stored            │
└────┬─────────────────────┘
     │ 6. Return blob URL
     ▼
┌──────────────────────────┐
│ TeacherDocumentRepository│
└────┬─────────────────────┘
     │ 7. Save metadata to database
     │    - TeacherId
     │    - DocumentType
     │    - BlobUrl
     │    - FileName
     │    - FileSize
     │    - UploadDate
     ▼
┌──────────────────────────┐
│ SQLite Database          │
│ ✓ Record saved           │
└────┬─────────────────────┘
     │ 8. Return success response
     ▼
┌──────────────────────────┐
│ Frontend                 │
│ ✓ Show success message   │
│ ✓ Refresh document list  │
└──────────────────────────┘
```

### 2. Document Download Flow

```
┌──────────┐
│  User    │
└────┬─────┘
     │ 1. Click "Download" button
     ▼
┌──────────────────┐
│  Document List   │
│  (Frontend)      │
└────┬─────────────┘
     │ 2. GET /api/teacherdocument/{id}/download
     ▼
┌──────────────────────────┐
│ TeacherDocumentController│
└────┬─────────────────────┘
     │ 3. Verify user access
     ▼
┌──────────────────────────┐
│ TeacherDocumentService   │
└────┬─────────────────────┘
     │ 4. Get document metadata
     ▼
┌──────────────────────────┐
│ DocumentStorageService   │
└────┬─────────────────────┘
     │ 5. Download from Azure Blob
     ▼
┌──────────────────────────┐
│ Azure Blob Storage       │
│ ✓ File retrieved         │
└────┬─────────────────────┘
     │ 6. Stream file content
     ▼
┌──────────────────────────┐
│ Frontend                 │
│ ✓ File downloaded        │
└──────────────────────────┘
```

### 3. Email Document Flow

```
┌──────────┐
│  User    │
└────┬─────┘
     │ 1. Click "Send Email" button
     ▼
┌──────────────────┐
│  Email Dialog    │
│  (Frontend)      │
└────┬─────────────┘
     │ 2. Enter recipient details
     │    - Email address
     │    - Name
     │    - Message
     ▼
┌──────────────────┐
│  Submit Form     │
└────┬─────────────┘
     │ 3. POST /api/teacherdocument/{id}/send-email
     ▼
┌──────────────────────────┐
│ TeacherDocumentController│
└────┬─────────────────────┘
     │ 4. Validate request
     ▼
┌──────────────────────────┐
│ TeacherDocumentService   │
└────┬─────────────────────┘
     │ 5. Get document details
     ▼
┌──────────────────────────┐
│ DocumentStorageService   │
└────┬─────────────────────┘
     │ 6. Download file from Azure
     ▼
┌──────────────────────────┐
│ EmailService             │
│ (Azure Communication)    │
└────┬─────────────────────┘
     │ 7. Prepare email
     │    - Subject
     │    - Body (HTML)
     │    - Attachment
     ▼
┌──────────────────────────┐
│ Azure Communication      │
│ Services (Email)         │
└────┬─────────────────────┘
     │ 8. Send email
     ▼
┌──────────────────────────┐
│ Recipient's Email        │
│ ✓ Email delivered        │
└────┬─────────────────────┘
     │ 9. Return success
     ▼
┌──────────────────────────┐
│ Frontend                 │
│ ✓ Show success message   │
└──────────────────────────┘
```

## Database Schema

### Teachers Table
```sql
┌─────────────────────────────────────────┐
│              Teachers                    │
├─────────────────────────────────────────┤
│ Id (PK)              INTEGER             │
│ TeacherName          VARCHAR(100)        │
│ Email                VARCHAR(100)        │
│ ContactNumber        VARCHAR(15)         │
│ Address              VARCHAR(500)        │
│ District             VARCHAR(50)         │
│ Pincode              VARCHAR(10)         │
│ SchoolId (FK)        INTEGER             │
│ ClassTeaching        VARCHAR(50)         │
│ Subject              VARCHAR(100)        │
│ Qualification        VARCHAR(100)        │
│ DateOfJoining        DATETIME            │
│ Salary               DECIMAL             │
│ IsActive             BIT                 │
└─────────────────────────────────────────┘
                │
                │ 1:N
                ▼
┌─────────────────────────────────────────┐
│         TeacherDocuments                 │
├─────────────────────────────────────────┤
│ Id (PK)              INTEGER             │
│ TeacherId (FK)       INTEGER             │
│ DocumentType         VARCHAR(50)         │
│ CustomDocumentType   VARCHAR(100)        │
│ FileName             VARCHAR(255)        │
│ OriginalFileName     VARCHAR(255)        │
│ BlobUrl              VARCHAR(1000)       │
│ BlobContainerName    VARCHAR(100)        │
│ BlobFileName         VARCHAR(255)        │
│ ContentType          VARCHAR(100)        │
│ FileSizeInBytes      BIGINT              │
│ Remarks              VARCHAR(500)        │
│ UploadedDate         DATETIME            │
│ UploadedByUserId     INTEGER             │
│ IsActive             BIT                 │
│ CreatedDate          DATETIME            │
│ UpdatedDate          DATETIME            │
└─────────────────────────────────────────┘
```

## Azure Blob Storage Structure

```
Storage Account: teacherdocsstorage
│
└── Container: teacher-documents
    │
    ├── Teacher ID: 1
    │   ├── Resume
    │   │   ├── a1b2c3d4-e5f6-7890-abcd-ef1234567890.pdf
    │   │   └── b2c3d4e5-f6g7-8901-bcde-fg2345678901.pdf
    │   │
    │   ├── Matric
    │   │   └── c3d4e5f6-g7h8-9012-cdef-gh3456789012.jpg
    │   │
    │   ├── Graduate
    │   │   └── d4e5f6g7-h8i9-0123-defg-hi4567890123.pdf
    │   │
    │   └── Other
    │       └── e5f6g7h8-i9j0-1234-efgh-ij5678901234.docx
    │
    ├── Teacher ID: 2
    │   ├── Resume
    │   │   └── f6g7h8i9-j0k1-2345-fghi-jk6789012345.pdf
    │   └── Inter
    │       └── g7h8i9j0-k1l2-3456-ghij-kl7890123456.pdf
    │
    └── Teacher ID: 3
        └── PG
            └── h8i9j0k1-l2m3-4567-hijk-lm8901234567.pdf
```

## Security Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Security Layers                           │
└─────────────────────────────────────────────────────────────┘

Layer 1: Authentication
┌─────────────────────────────────────────────────────────────┐
│  JWT Token Authentication                                    │
│  - User must login to get token                             │
│  - Token expires after 60 minutes                           │
│  - Token contains user ID and role                          │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
Layer 2: Authorization
┌─────────────────────────────────────────────────────────────┐
│  API Endpoint Protection                                     │
│  - All document endpoints require [Authorize]               │
│  - User ID extracted from JWT claims                        │
│  - Access logged with user ID                               │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
Layer 3: File Validation
┌─────────────────────────────────────────────────────────────┐
│  Upload Validation                                           │
│  - File size limit: 10 MB (configurable)                    │
│  - Allowed types: PDF, JPG, PNG, DOCX, etc.                │
│  - Filename sanitization                                     │
│  - Virus scanning (recommended for production)              │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
Layer 4: Azure Security
┌─────────────────────────────────────────────────────────────┐
│  Azure Blob Storage                                          │
│  - Private container (no anonymous access)                  │
│  - Encrypted at rest                                         │
│  - HTTPS only                                                │
│  - Connection string secured                                 │
│                                                              │
│  Azure Communication Services                                │
│  - Verified sender domain                                    │
│  - Rate limiting                                             │
│  - Email validation                                          │
│  - Connection string secured                                 │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
Layer 5: Database Security
┌─────────────────────────────────────────────────────────────┐
│  SQLite Database                                             │
│  - Soft delete (IsActive flag)                              │
│  - Audit trail (CreatedDate, UpdatedDate)                   │
│  - User tracking (UploadedByUserId)                         │
│  - Foreign key constraints                                   │
└─────────────────────────────────────────────────────────────┘
```

## API Endpoints Summary

```
┌─────────────────────────────────────────────────────────────┐
│                  Document Management API                     │
└─────────────────────────────────────────────────────────────┘

POST   /api/teacherdocument/upload
       ↳ Upload new document
       ↳ Auth: Required
       ↳ Input: multipart/form-data
       ↳ Output: TeacherDocumentDto

GET    /api/teacherdocument/teacher/{teacherId}
       ↳ Get all documents for a teacher
       ↳ Auth: Required
       ↳ Output: List<TeacherDocumentDto>

GET    /api/teacherdocument/{id}
       ↳ Get document details
       ↳ Auth: Required
       ↳ Output: TeacherDocumentDto

GET    /api/teacherdocument/{id}/download
       ↳ Download document file
       ↳ Auth: Required
       ↳ Output: File stream

DELETE /api/teacherdocument/{id}
       ↳ Delete document (soft delete)
       ↳ Auth: Required
       ↳ Output: Success message

POST   /api/teacherdocument/search
       ↳ Search documents with filters
       ↳ Auth: Required
       ↳ Input: DocumentSearchRequest
       ↳ Output: PagedResult<TeacherDocumentDto>

POST   /api/teacherdocument/{id}/send-email
       ↳ Send document via email
       ↳ Auth: Required
       ↳ Input: SendDocumentEmailDto
       ↳ Output: Success message
```

## Technology Stack

```
┌─────────────────────────────────────────────────────────────┐
│                      Frontend                                │
├─────────────────────────────────────────────────────────────┤
│  Framework:     Angular 21                                   │
│  Language:      TypeScript                                   │
│  UI Library:    Fluent UI (Educational Theme)               │
│  HTTP Client:   HttpClient (RxJS)                           │
│  Routing:       Angular Router                              │
│  Forms:         Reactive Forms                              │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                      Backend                                 │
├─────────────────────────────────────────────────────────────┤
│  Framework:     ASP.NET Core 10                             │
│  Language:      C# 12                                        │
│  Architecture:  Clean Architecture                          │
│  ORM:           Entity Framework Core                       │
│  Database:      SQLite                                       │
│  Auth:          JWT Bearer Tokens                           │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                    Azure Services                            │
├─────────────────────────────────────────────────────────────┤
│  Storage:       Azure Blob Storage                          │
│  Email:         Azure Communication Services                │
│  Hosting:       Azure App Service (for deployment)         │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                    NuGet Packages                            │
├─────────────────────────────────────────────────────────────┤
│  Azure.Storage.Blobs              12.19.1                   │
│  Azure.Communication.Email         1.0.1                    │
│  Microsoft.EntityFrameworkCore     9.0.0                    │
│  Microsoft.AspNetCore.Authentication.JwtBearer              │
└─────────────────────────────────────────────────────────────┘
```

## Performance Considerations

### File Upload Optimization
- **Streaming**: Files streamed directly to Azure (no temp storage)
- **Async Operations**: All I/O operations are async
- **Chunked Upload**: Large files uploaded in chunks
- **Progress Tracking**: Real-time upload progress

### Database Optimization
- **Indexes**: Created on TeacherId, DocumentType, UploadedDate
- **Pagination**: All list queries support pagination
- **Lazy Loading**: Documents loaded on-demand
- **Caching**: Frequently accessed data cached

### Azure Optimization
- **CDN**: Consider Azure CDN for document delivery
- **Blob Tiers**: Use Hot tier for recent docs, Cool for archives
- **Compression**: Enable compression for large files
- **Geo-Replication**: Use GRS for disaster recovery

---

**For setup instructions, see:**
- [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)
- [AZURE_QUICK_REFERENCE.md](AZURE_QUICK_REFERENCE.md)
