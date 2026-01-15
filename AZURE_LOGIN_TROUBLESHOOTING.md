# Azure Login 401 Error Troubleshooting Guide
## Bihar Teacher Management Portal

## 🚨 Current Issue
**URL**: `https://biharteacherportal-g6dhfcd3dzdahkc3.centralindia-01.azurewebsites.net/api/auth/login`
**Status**: `401 Unauthorized`
**Source**: Login attempt from frontend

## 🔍 Root Cause Analysis

The 401 error on login can be caused by several issues:

### 1. **Missing JWT Configuration in Azure** (Most Likely)
The JWT settings are not configured in Azure App Service.

### 2. **Invalid Login Credentials**
Using wrong username/password for login.

### 3. **Database Connection Issues**
SQLite database not accessible or missing.

### 4. **Application Startup Failure**
Backend service not starting properly due to configuration issues.

## 🛠️ Step-by-Step Fix

### **Step 1: Check Azure App Service Configuration**

1. **Go to Azure Portal**: https://portal.azure.com
2. **Navigate to**: Your App Service → Configuration → Application settings
3. **Verify these settings exist**:

| Setting Name | Required Value | Status |
|--------------|----------------|---------|
| `JwtSettings__SecretKey` | `YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters` | ❌ **MISSING** |
| `JwtSettings__Issuer` | `BiharTeacherPortal` | ❌ **MISSING** |
| `JwtSettings__Audience` | `BiharTeacherPortalUsers` | ❌ **MISSING** |

### **Step 2: Add Missing JWT Configuration**

**In Azure Portal → Your App Service → Configuration → Application settings**:

Click **"+ New application setting"** for each:

#### Setting 1:
- **Name**: `JwtSettings__SecretKey`
- **Value**: `YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters`

#### Setting 2:
- **Name**: `JwtSettings__Issuer`
- **Value**: `BiharTeacherPortal`

#### Setting 3:
- **Name**: `JwtSettings__Audience`
- **Value**: `BiharTeacherPortalUsers`

**Click "Save" after adding all settings.**

### **Step 3: Restart App Service**

1. **Go to**: Your App Service → Overview
2. **Click**: "Restart"
3. **Wait**: 2-3 minutes for restart to complete

### **Step 4: Test Default User Credentials**

Try logging in with these default credentials:

**Option 1 - Register New User First**:
- Go to: `https://biharteacherportal-g6dhfcd3dzdahkc3.centralindia-01.azurewebsites.net`
- Click "Register" 
- Create a new account
- Then try logging in

**Option 2 - Check if Default User Exists**:
The application might have seeded a default user. Common defaults:
- **Username**: `admin@example.com`
- **Password**: `Admin123!`

### **Step 5: Check Application Logs**

1. **Go to**: Azure Portal → Your App Service → Log stream
2. **Look for errors** related to:
   - JWT configuration
   - Database connection
   - Application startup

## 🔧 Alternative: Use Azure CLI to Set Configuration

If you have Azure CLI installed:

```bash
# Set your app name and resource group
APP_NAME="biharteacherportal-g6dhfcd3dzdahkc3"
RESOURCE_GROUP="your-resource-group-name"

# Configure JWT settings
az webapp config appsettings set \
  --resource-group $RESOURCE_GROUP \
  --name $APP_NAME \
  --settings \
    "JwtSettings__SecretKey=YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters" \
    "JwtSettings__Issuer=BiharTeacherPortal" \
    "JwtSettings__Audience=BiharTeacherPortalUsers"

# Restart the app
az webapp restart --resource-group $RESOURCE_GROUP --name $APP_NAME
```

## 🧪 Testing After Fix

### Test 1: Check App Service Health
Visit: `https://biharteacherportal-g6dhfcd3dzdahkc3.centralindia-01.azurewebsites.net`
- Should load the Angular application without errors

### Test 2: Test Registration
1. Click "Register" 
2. Fill in details:
   - **Email**: `test@example.com`
   - **Password**: `Test123!`
3. Should get success message

### Test 3: Test Login
1. Click "Login"
2. Use the credentials you just registered
3. Should successfully log in and redirect to dashboard

### Test 4: Check Browser Network Tab
1. Open Developer Tools → Network
2. Try logging in
3. Check the `/api/auth/login` request:
   - Should return `200 OK` with token
   - Response should contain `{ "token": "...", "userName": "..." }`

## 🚨 If Still Not Working

### Check These Additional Issues:

#### 1. Database File Missing
- Ensure `authapp.db` is included in deployment
- Check if SQLite file has proper permissions

#### 2. Connection String Issues
Add this application setting if needed:
- **Name**: `ConnectionStrings__DefaultConnection`
- **Value**: `Data Source=authapp.db`

#### 3. CORS Issues
Add these if needed:
- **Name**: `ASPNETCORE_ENVIRONMENT`
- **Value**: `Production`

#### 4. Application Startup Issues
Check logs for:
- Missing dependencies
- Configuration errors
- Database migration issues

## 📞 Quick Verification Commands

### Test API Directly with curl:

```bash
# Test registration
curl -X POST https://biharteacherportal-g6dhfcd3dzdahkc3.centralindia-01.azurewebsites.net/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail":"test@example.com","password":"Test123!"}'

# Test login
curl -X POST https://biharteacherportal-g6dhfcd3dzdahkc3.centralindia-01.azurewebsites.net/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail":"test@example.com","password":"Test123!"}'
```

## ✅ Expected Results After Fix

1. **Registration**: Returns `200 OK` with success message
2. **Login**: Returns `200 OK` with JWT token and username
3. **Frontend**: Login button works, redirects to dashboard
4. **Authentication**: Protected pages accessible after login

## 🔐 Security Note

The JWT secret key should be:
- **At least 32 characters long**
- **Unique for your application**
- **Kept secret** (never commit to source control)
- **Different for production** than development

For production, consider using a more secure key like:
`BiharTeacherPortal2024SecureJWTKeyForProductionEnvironment123456789`

---

**Most likely fix**: Add the missing JWT configuration settings in Azure App Service and restart the application! 🚀