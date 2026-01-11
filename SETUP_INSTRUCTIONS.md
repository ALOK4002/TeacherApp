# Setup Instructions

## Prerequisites
- .NET 10 SDK
- Node.js 24
- Angular CLI 21

## Backend Setup

1. Navigate to the Backend directory:
```bash
cd Backend
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Create the database (if not already created):
```bash
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

4. Run the backend API:
```bash
dotnet run --project WebAPI
```

The backend will be available at: https://localhost:7001

## Frontend Setup

1. Navigate to the Frontend directory:
```bash
cd Frontend
```

2. Install npm packages:
```bash
npm install
```

3. Start the Angular development server:
```bash
ng serve
```

The frontend will be available at: http://localhost:4200

## Testing the Application

1. Open your browser and go to http://localhost:4200
2. You'll be redirected to the registration page
3. Register a new user with username/email and password (minimum 6 characters)
4. After successful registration, you'll be redirected to the login page
5. Login with your credentials
6. You'll be redirected to the welcome page showing your username

## API Endpoints

- POST `/api/auth/register` - Register a new user
- POST `/api/auth/login` - Login user and get JWT token

## Database

The application uses SQLite database (`authapp.db`) which will be created automatically in the WebAPI project directory.

## Architecture

### Backend (Clean Architecture)
- **Domain**: Core entities and interfaces
- **Application**: Business logic, DTOs, and validation
- **Infrastructure**: Data access, repositories, and external services
- **WebAPI**: Controllers and API configuration

### Frontend (Angular 21)
- **Components**: Register, Login, Welcome
- **Services**: AuthService for API communication
- **Interceptors**: JWT token attachment
- **Models**: TypeScript interfaces for data transfer

## Security Features

- Password hashing using SHA256
- JWT token authentication
- CORS configuration for Angular frontend
- Form validation on both client and server side