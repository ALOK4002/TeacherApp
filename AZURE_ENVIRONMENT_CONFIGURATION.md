# Azure Environment Configuration Guide
## Bihar Teacher Management Portal

This guide explains how to configure environment variables and settings for Azure deployment to ensure the frontend can properly communicate with the backend API.

## 🔧 Problem Fixed

**Issue**: After Azure deployment, frontend clicks don't work because services were hardcoded to use `http://localhost:5162`

**Solution**: Updated all services to use environment-based configuration that adapts to deployment environment.

## 📁 Files Updated

### Frontend Environment Files
- ✅ `Frontend/src/environments/environment.ts` - Development configuration
- ✅ `Frontend/src/environments/environment.prod.ts` - Production configuration
- ✅ `Frontend/angular.json` - Build configuration with file replacements

### Services Updated
- ✅ `Frontend/src/app/services/auth.service.ts`
- ✅ `Frontend/src/app/services/school.service.ts`
- ✅ `Frontend/src/app/services/teacher.service.ts`
- ✅ `Frontend/src/app/services/notice.service.ts`
- ✅ `Frontend/src/app/services/search.service.ts` (already correct)

## 🌍 Environment Configuration

### Development Environment (`environment.ts`)
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5162/api'
};
```

### Production Environment (`environment.prod.ts`)
```typescript
export const environment = {
  production: true,
  apiUrl: '/api'  // Relative URL - uses same domain as frontend
};
```

## 🚀 Azure App Service Configuration

### No Additional Configuration Needed!

Since the frontend is served from the same domain as the backend (both from your Azure App Service), using relative URLs (`/api`) means:

- **Frontend URL**: `https://your-app.azurewebsites.net`
- **API Calls**: `https://your-app.azurewebsites.net/api/*`

This automatically works without any additional Azure configuration!

## 🔍 Azure Parameters to Check

If you encounter issues, verify these settings in Azure Portal:

### 1. App Service Configuration
**Location**: Azure Portal → Your App Service → Configuration → Application settings

**Check these settings exist**:
- `WEBSITE_RUN_FROM_PACKAGE`: `1` (if using zip deployment)
- `SCM_DO_BUILD_DURING_DEPLOYMENT`: `true` (if needed)

### 2. Connection Strings
**Location**: Azure Portal → Your App Service → Configuration → Connection strings

**Verify**:
- `DefaultConnection`: Your SQLite connection string
- Type: `SQLite`

### 3. General Settings
**Location**: Azure Portal → Your App Service → Configuration → General settings

**Check**:
- **Stack**: `.NET`
- **Major version**: `10` (for .NET 10)
- **Platform**: `64 Bit`
- **Always On**: `On` (for production)

### 4. CORS Settings (if needed)
**Location**: Azure Portal → Your App Service → CORS

**Settings**:
- **Allowed Origins**: `*` (or your specific domain)
- **Allow Credentials**: `false`

## 🛠️ Deployment Process

### 1. Build Frontend for Production
```bash
cd Frontend
npm run build
```
This creates optimized files in `Backend/WebAPI/wwwroot` using production environment.

### 2. Deploy Backend
```bash
cd Backend
dotnet publish -c Release -o ./publish
```

### 3. Upload to Azure
- Zip the `publish` folder contents
- Upload via Azure Portal or use deployment scripts

## 🧪 Testing After Deployment

### 1. Test Frontend Loading
Visit: `https://your-app.azurewebsites.net`
- Should load the Angular application
- Check browser console for errors

### 2. Test API Endpoints
Open browser developer tools and check network requests:
- Login should call: `https://your-app.azurewebsites.net/api/auth/login`
- Schools should call: `https://your-app.azurewebsites.net/api/school`

### 3. Test Authentication Flow
1. Try to login
2. Check if JWT token is stored
3. Navigate to protected pages
4. Verify API calls include Authorization header

## 🚨 Common Issues & Solutions

### Issue 1: "Failed to load resource" errors
**Cause**: API calls going to wrong URL
**Solution**: Verify environment.prod.ts is being used in production build

### Issue 2: CORS errors
**Cause**: Cross-origin requests blocked
**Solution**: Since frontend and backend are same domain, this shouldn't happen. Check if API URLs are correct.

### Issue 3: 404 errors on API calls
**Cause**: API routes not found
**Solution**: 
- Check backend is running
- Verify controller routes are correct
- Check if database is accessible

### Issue 4: Authentication not working
**Cause**: JWT token issues
**Solution**:
- Check JWT settings in appsettings.json
- Verify token is being stored in localStorage
- Check Authorization header in requests

## 🔧 Debug Commands

### Check Environment in Browser
Open browser console and run:
```javascript
// This will show which environment is being used
console.log('Environment:', window.location.origin);
```

### Check API Calls
In browser developer tools → Network tab:
- Filter by "XHR" to see API calls
- Check request URLs are going to correct domain
- Verify Authorization headers are present

### Check Local Storage
In browser developer tools → Application → Local Storage:
- Should see `token` and `userName` after login
- Values should be valid JWT token and username

## 📋 Deployment Checklist

Before deploying to Azure:

- [ ] Frontend built with production configuration (`npm run build`)
- [ ] All services use environment.apiUrl instead of hardcoded URLs
- [ ] Environment.prod.ts has correct API URL (`/api`)
- [ ] Angular.json has fileReplacements for production
- [ ] Backend appsettings.json has correct connection strings
- [ ] SQLite database file is included in deployment
- [ ] JWT settings are configured
- [ ] CORS is properly configured

After deploying to Azure:

- [ ] Frontend loads without errors
- [ ] Login functionality works
- [ ] API calls go to correct URLs
- [ ] Authentication persists across page refreshes
- [ ] All CRUD operations work
- [ ] Search functionality works (if Azure Cognitive Search is configured)

## 🎯 Key Benefits

1. **Environment-Aware**: Automatically uses correct API URLs based on environment
2. **No CORS Issues**: Frontend and backend served from same domain
3. **Simplified Deployment**: No additional Azure configuration needed
4. **Maintainable**: Easy to update API URLs for different environments
5. **Secure**: Uses relative URLs, no hardcoded domains

The configuration is now production-ready and will work seamlessly in Azure App Service! 🚀