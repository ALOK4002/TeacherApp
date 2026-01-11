# Testing Guide for Authentication API

## Backend API Endpoints
- **HTTPS**: https://localhost:7014
- **HTTP**: http://localhost:5162

## Frontend
- **Angular Dev Server**: http://localhost:4200

## Step 1: Start the Backend API

```bash
cd Backend
dotnet run --project WebAPI
```

You should see output similar to:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5162
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7014
```

## Step 2: Test API Endpoints Manually

### Using curl commands:

#### Test HTTPS endpoint (7014):
```bash
# Test registration
curl -X POST https://localhost:7014/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser", "password": "password123"}' \
  -k

# Test login
curl -X POST https://localhost:7014/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser", "password": "password123"}' \
  -k
```

#### Test HTTP endpoint (5162):
```bash
# Test registration
curl -X POST http://localhost:5162/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser2", "password": "password123"}'

# Test login
curl -X POST http://localhost:5162/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser2", "password": "password123"}'
```

### Using Browser (Swagger UI):

1. Open your browser and navigate to:
   - **HTTPS**: https://localhost:7014/swagger
   - **HTTP**: http://localhost:5162/swagger

2. You'll see the Swagger UI with the authentication endpoints
3. Click on "POST /api/auth/register" to expand it
4. Click "Try it out" and enter test data:
   ```json
   {
     "userNameOrEmail": "testuser",
     "password": "password123"
   }
   ```
5. Click "Execute" to test the registration
6. Repeat for the login endpoint

## Step 3: Test with Postman

### Registration Request:
- **Method**: POST
- **URL**: https://localhost:7014/api/auth/register (or http://localhost:5162/api/auth/register)
- **Headers**: Content-Type: application/json
- **Body** (raw JSON):
```json
{
  "userNameOrEmail": "testuser",
  "password": "password123"
}
```

### Login Request:
- **Method**: POST
- **URL**: https://localhost:7014/api/auth/login (or http://localhost:5162/api/auth/login)
- **Headers**: Content-Type: application/json
- **Body** (raw JSON):
```json
{
  "userNameOrEmail": "testuser",
  "password": "password123"
}
```

## Step 4: Start Frontend and Test Full Flow

```bash
cd Frontend
npm install
ng serve
```

1. Open browser to http://localhost:4200
2. Register a new user
3. Login with the registered user
4. Verify you reach the welcome page

## Expected Responses

### Successful Registration:
```json
{
  "message": "User registered successfully"
}
```

### Successful Login:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userName": "testuser"
}
```

### Error Responses:
```json
{
  "message": "User already exists"
}
```

```json
{
  "message": "Invalid credentials"
}
```

## Troubleshooting

### If HTTPS (7014) doesn't work:
- The SSL certificate might not be trusted
- Use the `-k` flag with curl to ignore SSL errors
- In browser, you might need to accept the security warning

### If HTTP (5162) doesn't work:
- Check if the port is already in use
- Verify the backend is running with `dotnet run --project WebAPI`

### If CORS errors occur:
- Ensure the CORS policy includes your frontend URL
- Check browser developer tools for specific CORS error messages

### Database Issues:
- If you get database errors, run:
```bash
cd Backend
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

## Verification Checklist

- [ ] Backend starts without errors
- [ ] Can access Swagger UI at both ports
- [ ] Registration endpoint works via curl/Postman
- [ ] Login endpoint works via curl/Postman
- [ ] Frontend connects to backend successfully
- [ ] Full registration → login → welcome flow works
- [ ] JWT token is returned on successful login
- [ ] Database file (authapp.db) is created in WebAPI directory