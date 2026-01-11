# Project Structure

## Backend (.NET 10 Clean Architecture)

```
Backend/
├── AuthApp.sln                    # Solution file
├── Domain/                        # Core business layer
│   ├── Entities/
│   │   └── User.cs                # User entity
│   └── Interfaces/
│       └── IUserRepository.cs     # Repository interface
├── Application/                   # Business logic layer
│   ├── DTOs/
│   │   ├── RegisterRequestDto.cs
│   │   ├── LoginRequestDto.cs
│   │   └── AuthResponseDto.cs
│   ├── Interfaces/
│   │   └── IAuthService.cs        # Auth service interface
│   └── Validators/
│       ├── RegisterRequestValidator.cs
│       └── LoginRequestValidator.cs
├── Infrastructure/                # Data access layer
│   ├── Persistence/
│   │   └── AppDbContext.cs        # EF Core DbContext
│   ├── Repositories/
│   │   └── UserRepository.cs      # User repository implementation
│   ├── Services/
│   │   ├── AuthService.cs         # Auth service implementation
│   │   ├── PasswordService.cs     # Password hashing
│   │   └── JwtService.cs          # JWT token generation
│   └── Migrations/                # EF Core migrations
└── WebAPI/                        # API layer
    ├── Controllers/
    │   └── AuthController.cs      # Authentication endpoints
    ├── Program.cs                 # Application configuration
    ├── appsettings.json          # Configuration settings
    └── authapp.db                # SQLite database file
```

## Frontend (Angular 21)

```
Frontend/
├── src/
│   └── app/
│       ├── components/
│       │   ├── register/
│       │   │   └── register.component.ts
│       │   ├── login/
│       │   │   └── login.component.ts
│       │   └── welcome/
│       │       └── welcome.component.ts
│       ├── services/
│       │   └── auth.service.ts    # HTTP client service
│       ├── interceptors/
│       │   └── auth.interceptor.ts # JWT token interceptor
│       ├── models/
│       │   └── auth.models.ts     # TypeScript interfaces
│       ├── app.routes.ts          # Routing configuration
│       ├── app.config.ts          # Application configuration
│       └── app.ts                 # Root component
├── package.json                   # npm dependencies
└── angular.json                   # Angular CLI configuration
```

## Key Features

### Backend
- **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure, and WebAPI layers
- **Entity Framework Core**: Code-first approach with SQLite database
- **JWT Authentication**: Secure token-based authentication
- **FluentValidation**: Server-side validation for DTOs
- **CORS Configuration**: Allows Angular frontend communication
- **Password Hashing**: SHA256 password security
- **Swagger/OpenAPI**: API documentation

### Frontend
- **Angular 21**: Latest Angular framework with standalone components
- **Reactive Forms**: Form validation and user input handling
- **HTTP Interceptors**: Automatic JWT token attachment
- **Routing**: Navigation between Register → Login → Welcome
- **Local Storage**: Token persistence
- **Responsive Design**: Clean, modern UI
- **SSR Support**: Server-side rendering compatibility

## Database Schema

### Users Table
- `Id` (int, Primary Key, Auto-increment)
- `UserName` (string, Required, Unique, Max 100 chars)
- `Email` (string, Required, Unique, Max 255 chars)
- `PasswordHash` (string, Required)
- `CreatedDate` (DateTime, Required)

## API Endpoints

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User authentication

## Authentication Flow

1. User registers with username/email and password
2. Password is hashed and stored in database
3. User logs in with credentials
4. Server validates credentials and returns JWT token
5. Frontend stores token in localStorage
6. Token is automatically attached to subsequent API requests
7. Welcome page displays authenticated user information