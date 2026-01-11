# Full-Stack Authentication App

## Project Structure
- **Backend**: ASP.NET Core Web API (.NET 10) with Clean Architecture
- **Frontend**: Angular 21 with Node.js 24
- **Database**: SQLite (local)

## Backend Projects
1. **Domain** - Core entities and interfaces
2. **Application** - Business logic and DTOs
3. **Infrastructure** - Data access and external services
4. **WebAPI** - API controllers and configuration

## Frontend
- Angular 21 application with authentication flow
- Register → Login → Welcome pages

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
ng serve
```

The backend will run on https://localhost:7001 and frontend on http://localhost:4200