# Manual Azure Portal Deployment Guide
## Bihar Teacher Management Portal

This guide shows you how to manually deploy the application using Azure Portal without any command-line tools.

## ⚠️ IMPORTANT: Environment Configuration Fixed

**The application has been updated to fix API URL issues after deployment!**

- ✅ All services now use environment-based configuration
- ✅ Production build uses relative URLs (`/api`) instead of `localhost:5162`
- ✅ No additional Azure configuration needed for API communication

**See**: `AZURE_ENVIRONMENT_CONFIGURATION.md` for detailed technical information.

## 🔧 Part 1: Configure Angular Build (Already Done)

The Angular configuration has been updated to build directly to the backend's wwwroot folder:

### Angular Configuration Changes Made:
- ✅ **Output Path**: Changed to `../Backend/WebAPI/wwwroot`
- ✅ **Backend Configuration**: Updated to serve files from `wwwroot/browser`
- ✅ **Routing Fallback**: Configured for Angular SPA routing

### Build Command:
```bash
cd Frontend
npm run build
```
This now builds directly to: `Backend/WebAPI/wwwroot/`

## 🌐 Part 2: Manual Azure Portal Deployment

### Step 1: Login to Azure Portal
1. Go to [https://portal.azure.com](https://portal.azure.com)
2. Sign in with your Azure account
3. Ensure you have an active subscription

### Step 2: Create Resource Group
1. **Search** for "Resource groups" in the top search bar
2. Click **"+ Create"**
3. Fill in the details:
   - **Subscription**: Select your subscription
   - **Resource group name**: `bihar-teacher-portal-rg`
   - **Region**: `East US` (or your preferred region)
4. Click **"Review + create"** → **"Create"**

### Step 3: Create App Service Plan
1. **Search** for "App Service plans" in the search bar
2. Click **"+ Create"**
3. Fill in the details:
   - **Subscription**: Your subscription
   - **Resource Group**: `bihar-teacher-portal-rg`
   - **Name**: `bihar-teacher-portal-plan`
   - **Operating System**: `Linux`
   - **Region**: `East US` (same as resource group)
   - **Pricing Tier**: Click "Change size" → Select **"F1 (Free)"**
4. Click **"Review + create"** → **"Create"**

### Step 4: Create Web App
1. **Search** for "App Services" in the search bar
2. Click **"+ Create"** → **"Web App"**
3. Fill in the **Basics** tab:
   - **Subscription**: Your subscription
   - **Resource Group**: `bihar-teacher-portal-rg`
   - **Name**: `bihar-teacher-portal-[your-unique-suffix]` (must be globally unique)
   - **Publish**: `Code`
   - **Runtime stack**: `.NET 8 (LTS)`
   - **Operating System**: `Linux`
   - **Region**: `East US`
   - **App Service Plan**: Select the plan you created above
4. Click **"Review + create"** → **"Create"**
5. **Save the Web App URL** (e.g., `https://bihar-teacher-portal-123.azurewebsites.net`)

### Step 5: Configure Application Settings
1. Go to your **Web App** in the portal
2. In the left menu, click **"Configuration"**
3. Under **"Application settings"**, click **"+ New application setting"**
4. Add these settings one by one:

   **Setting 1:**
   - **Name**: `JwtSettings__SecretKey`
   - **Value**: `YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters`

   **Setting 2:**
   - **Name**: `JwtSettings__Issuer`
   - **Value**: `BiharTeacherPortal`

   **Setting 3:**
   - **Name**: `JwtSettings__Audience`
   - **Value**: `BiharTeacherPortalUsers`

   **Setting 4:**
   - **Name**: `ConnectionStrings__DefaultConnection`
   - **Value**: `Data Source=authapp.db`

   **Setting 5:**
   - **Name**: `ASPNETCORE_ENVIRONMENT`
   - **Value**: `Production`

   **Setting 6 (Azure Cognitive Search):**
   - **Name**: `AzureSearch__ServiceEndpoint`
   - **Value**: `https://your-search-service.search.windows.net`

   **Setting 7 (Azure Cognitive Search):**
   - **Name**: `AzureSearch__ApiKey`
   - **Value**: `your-admin-api-key`

5. Click **"Save"** at the top

**📊 Important Database Notes:**
- The SQLite database (`authapp.db`) is automatically included in your deployment package
- The database contains pre-seeded data (Bihar schools, districts, pincodes)
- Connection string points to `authapp.db` in the application root
- Database will be created automatically on first run if it doesn't exist
- All user data, schools, teachers, and notices are stored in this database

### Step 6: Enable Application Logging
1. In your Web App, go to **"App Service logs"** in the left menu
2. Set **"Application logging (Filesystem)"** to **"On"**
3. Set **"Level"** to **"Information"**
4. Click **"Save"**

## 📦 Part 3: Prepare Application for Deployment

### Step 1: Build Frontend (if not done already)
```bash
cd Frontend
npm install
npm run build
```
This builds to: `Backend/WebAPI/wwwroot/browser/`

### Step 2: Build Backend
```bash
cd Backend/WebAPI
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

### Step 3: Create Deployment Package
```bash
cd Backend/WebAPI/publish
zip -r ../deploy.zip .
cd ..
```

You should now have: `Backend/WebAPI/deploy.zip`

## 🚀 Part 4: Deploy Using Azure Portal

### Method 1: Deployment Center (Recommended)

1. In your **Web App**, go to **"Deployment Center"** in the left menu
2. Under **"Settings"** tab, select:
   - **Source**: `Local Git` or `External Git`
   - **Build Provider**: `App Service Build Service`
3. Click **"Save"**
4. Go to **"FTPS credentials"** tab and note the credentials
5. Use an FTP client or the portal's **"Advanced Tools (Kudu)"**

### Method 2: Advanced Tools (Kudu) - Easier

1. In your **Web App**, go to **"Advanced Tools"** in the left menu
2. Click **"Go"** (opens Kudu console)
3. Click **"Debug console"** → **"CMD"**
4. Navigate to: `site/wwwroot`
5. **Drag and drop** your `deploy.zip` file into the file manager
6. Click on `deploy.zip` and select **"Extract"**
7. Delete the `deploy.zip` file after extraction

### Method 3: ZIP Deploy via Portal

1. In your **Web App**, go to **"Advanced Tools"** → **"Go"**
2. In Kudu, go to: `https://your-app-name.scm.azurewebsites.net/ZipDeployUI`
3. **Drag and drop** your `deploy.zip` file
4. Wait for deployment to complete

### Method 4: Visual Studio Code (if you have it)

1. Install **"Azure App Service"** extension
2. Sign in to Azure
3. Right-click your Web App → **"Deploy to Web App"**
4. Select the `Backend/WebAPI/publish` folder

## 🔍 Part 5: Verify Deployment

### Step 1: Check Deployment Status
1. In your **Web App**, go to **"Deployment Center"**
2. Check the **"Logs"** tab for any errors
3. Ensure deployment shows as **"Success"**

### Step 2: Test the Application
1. Go to your Web App URL: `https://your-app-name.azurewebsites.net`
2. You should see the Angular application
3. Test user registration and login
4. Verify API endpoints work: `https://your-app-name.azurewebsites.net/api/auth/login`

### Step 3: Check Logs (if issues)
1. Go to **"Log stream"** in your Web App
2. Or go to **"Advanced Tools"** → **"Go"** → **"Debug console"**
3. Check logs in: `LogFiles/Application`

## 🛠️ Part 6: Troubleshooting

### Common Issues:

#### 1. **404 Error on Angular Routes**
- **Problem**: Direct navigation to Angular routes fails
- **Solution**: Verify `web.config` is deployed and contains rewrite rules

#### 2. **Static Files Not Loading**
- **Problem**: CSS/JS files return 404
- **Solution**: Check that files are in `wwwroot/browser/` folder

#### 3. **API Calls Failing**
- **Problem**: Frontend can't reach backend APIs
- **Solution**: Check CORS configuration in `Program.cs`

#### 4. **Database Errors**
- **Problem**: SQLite database issues
- **Solution**: Check connection string and file permissions

### Debug Commands (in Kudu Console):
```bash
# Check if files are deployed
ls -la site/wwwroot/

# Check if .NET is working
dotnet --version

# Check application logs
tail -f LogFiles/Application/applicationLog.txt
```

## 📋 Part 7: Post-Deployment Checklist

- [ ] ✅ Web app loads at your Azure URL
- [ ] ✅ User registration works
- [ ] ✅ User login works
- [ ] ✅ All Angular routes work (About, Teachers, Schools, Notices)
- [ ] ✅ API endpoints respond correctly
- [ ] ✅ Database operations work
- [ ] ✅ Static files (CSS, JS) load properly

## 🔄 Part 8: Future Updates

For future deployments:

1. **Build Frontend**: `cd Frontend && npm run build`
2. **Build Backend**: `cd Backend/WebAPI && dotnet publish -c Release -o ./publish`
3. **Create ZIP**: `cd publish && zip -r ../deploy.zip . && cd ..`
4. **Deploy**: Use Kudu or Deployment Center to upload `deploy.zip`

## 💡 Pro Tips

1. **Bookmark Kudu URL**: `https://your-app-name.scm.azurewebsites.net`
2. **Use Deployment Slots**: For zero-downtime deployments (requires paid tier)
3. **Monitor Performance**: Enable Application Insights
4. **Set up Alerts**: Configure alerts for errors or performance issues
5. **Backup Strategy**: Export app settings and database regularly

## 🎯 Summary

You now have:
- ✅ **Angular configured** to build directly to backend wwwroot
- ✅ **Complete manual deployment guide** using Azure Portal
- ✅ **Multiple deployment methods** (Kudu, ZIP deploy, VS Code)
- ✅ **Troubleshooting guide** for common issues
- ✅ **Future update process** for ongoing maintenance

Your Bihar Teacher Management Portal is ready for manual Azure deployment! 🚀