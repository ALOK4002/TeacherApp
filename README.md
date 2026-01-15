# Bihar Teacher Management Portal

## 🎓 Full-Stack Application with Document Management

A comprehensive teacher management system for Bihar schools with Azure-powered document storage and email capabilities.

## 📚 Documentation

### 🚀 Quick Start
- **[DOCUMENTATION_INDEX.md](DOCUMENTATION_INDEX.md)** - Complete documentation index
- **[HOW_TO_ACCESS_DOCUMENTS.md](HOW_TO_ACCESS_DOCUMENTS.md)** - Find the Documents button

### ⚡ Azure Setup
- **[README_AZURE_SETUP.md](README_AZURE_SETUP.md)** - Azure setup overview
- **[AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)** - Complete setup guide
- **[AZURE_QUICK_REFERENCE.md](AZURE_QUICK_REFERENCE.md)** - Quick reference card
- **[AZURE_SETUP_CHECKLIST.md](AZURE_SETUP_CHECKLIST.md)** - Printable checklist

### 🏗️ Technical Documentation
- **[DOCUMENT_MANAGEMENT_ARCHITECTURE.md](DOCUMENT_MANAGEMENT_ARCHITECTURE.md)** - System architecture
- **[TEACHER_DOCUMENT_MANAGEMENT_SETUP.md](TEACHER_DOCUMENT_MANAGEMENT_SETUP.md)** - Implementation details

## 🌟 Features

### Core Features
- ✅ User authentication (Register/Login with JWT)
- ✅ School management system
- ✅ Teacher management with Bihar districts
- ✅ Notice board with replies
- ✅ Teacher-School reports with pagination
- ✅ About Us page

### Document Management (NEW!)
- ✅ Upload multiple documents per teacher
- ✅ Azure Blob Storage integration
- ✅ Document types: Resume, Matric, Inter, Graduate, PG, Custom
- ✅ Download documents
- ✅ Send documents via email (Azure Communication Services)
- ✅ Search and filter documents
- ✅ Secure storage with audit trail

## 🏗️ Project Structure
- **Backend**: ASP.NET Core Web API (.NET 10) with Clean Architecture
- **Frontend**: Angular 21 with Fluent UI Educational Theme
- **Database**: SQLite (local development)
- **Cloud**: Azure Blob Storage + Azure Communication Services

## Backend Projects
1. **Domain** - Core entities and interfaces
2. **Application** - Business logic and DTOs
3. **Infrastructure** - Data access and external services
4. **WebAPI** - API controllers and configuration

## Frontend
- Angular 21 application with Fluent UI design
- Reactive forms and JWT authentication
- Document upload with progress tracking

## Setup Instructions

### Backend
```bash
cd Backend
dotnet restore
dotnet ef database update --project Infrastructure --startup-project WebAPI
dotnet run --project WebAPI
```

### Frontend
```bash
cd Frontend
npm install
npm start
```

The backend runs on http://localhost:5162 and frontend on http://localhost:4200