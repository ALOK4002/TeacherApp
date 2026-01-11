# Azure Deployment Guide - Bihar Teacher Management Portal

## Overview
This guide provides multiple deployment options for the Bihar Teacher Management Portal to Azure, with the frontend integrated into the backend for simplified deployment.

## 🏗️ Architecture Changes Made

### Frontend Integration
- ✅ Frontend built and copied to `Backend/WebAPI/wwwroot/`
- ✅ Backend configured to serve static files
- ✅ Angular routing fallback configured
- ✅ CORS updated for production

### Backend Configuration
- ✅ Static file serving enabled
- ✅ Fallback routing for Angular SPA
- ✅ Production-ready CORS policy
- ✅ Azure-compatible configuration

## 🚀 Deployment Options

### Option 1: Azure App Service (Recommended)

#### Prerequisites
- Azure CLI installed and configured
- Azure subscription with appropriate permissions

#### Steps

1. **Create Azure Resources**
   ```bash
   ./deploy-to-azure.sh
   ```
   This creates:
   - Resource Group: `bihar-teacher-portal-rg`
   - App Service Plan: `bihar-teacher-portal-plan` (Free tier)
   - Web App: `bihar-teacher-portal-<timestamp>`

2. **Deploy Application**
   ```bash
   ./deploy-app.sh <web-app-name>
   ```
   This will:
   - Build Angular frontend
   - Copy frontend to backend wwwroot
   - Build and publish .NET backend
   - Deploy to Azure App Service

#### Manual Deployment Steps
If you prefer manual deployment:

```bash
# 1. Build Frontend
cd Frontend
npm install
npm run build
cd ..

# 2. Copy to Backend
mkdir -p Backend/WebAPI/wwwroot
cp -r Frontend/dist/Frontend/* Backend/WebAPI/wwwroot/

# 3. Publish Backend
cd Backend/WebAPI
dotnet publish -c Release -o ./publish

# 4. Deploy to Azure
cd publish
zip -r ../deploy.zip .
az webapp deployment source config-zip --resource-group bihar-teacher-portal-rg --name <your-app-name> --src ../deploy.zip
```

### Option 2: Docker Container Deployment

#### Using Azure Container Instances
```bash
./deploy-container.sh
```

#### Using Docker Locally
```bash
# Build the image
docker build -t bihar-teacher-portal -f Backend/WebAPI/Dockerfile .

# Run locally
docker run -p 8080:80 bihar-teacher-portal
```

### Option 3: GitHub Actions CI/CD

1. **Setup Repository Secrets**
   - `AZURE_WEBAPP_PUBLISH_PROFILE`: Download from Azure Portal

2. **Use the provided workflow**
   - Copy `azure-deploy.yml` to `.github/workflows/`
   - Update the `AZURE_WEBAPP_NAME` in the workflow file
   - Push to main branch to trigger deployment

## ⚙️ Configuration

### Environment Variables
Set these in Azure App Service Configuration:

```
JwtSettings__SecretKey=YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters
JwtSettings__Issuer=BiharTeacherPortal
JwtSettings__Audience=BiharTeacherPortalUsers
ConnectionStrings__DefaultConnection=Data Source=app.db
ASPNETCORE_ENVIRONMENT=Production
```

### Database Configuration
The application uses SQLite by default. For production, consider:
- Azure SQL Database
- PostgreSQL on Azure
- MySQL on Azure

Update the connection string accordingly.

## 🔧 Files Created for Deployment

### Backend Configuration
- `Backend/WebAPI/web.config` - IIS configuration for Azure App Service
- `Backend/WebAPI/Dockerfile` - Container deployment
- `Backend/WebAPI/Program.cs` - Updated with static file serving

### Deployment Scripts
- `deploy-to-azure.sh` - Creates Azure resources
- `deploy-app.sh` - Deploys the application
- `deploy-container.sh` - Container deployment
- `azure-deploy.yml` - GitHub Actions workflow

## 🌐 Post-Deployment

### Verify Deployment
1. **Check Application Status**
   ```bash
   az webapp show --resource-group bihar-teacher-portal-rg --name <your-app-name> --query state
   ```

2. **View Logs**
   ```bash
   az webapp log tail --resource-group bihar-teacher-portal-rg --name <your-app-name>
   ```

3. **Open in Browser**
   ```bash
   az webapp browse --resource-group bihar-teacher-portal-rg --name <your-app-name>
   ```

### Test the Application
- Navigate to your Azure app URL
- Test user registration and login
- Verify all features work correctly
- Check API endpoints at `/api/`

## 🔒 Security Considerations

### Production Checklist
- [ ] Update JWT secret key
- [ ] Configure HTTPS only
- [ ] Set up custom domain (optional)
- [ ] Configure Application Insights
- [ ] Set up backup strategy
- [ ] Configure scaling rules

### Environment Security
```bash
# Enable HTTPS only
az webapp update --resource-group bihar-teacher-portal-rg --name <your-app-name> --https-only true

# Configure minimum TLS version
az webapp config set --resource-group bihar-teacher-portal-rg --name <your-app-name> --min-tls-version 1.2
```

## 📊 Monitoring and Maintenance

### Application Insights
```bash
# Enable Application Insights
az monitor app-insights component create --app <your-app-name> --location "East US" --resource-group bihar-teacher-portal-rg
```

### Scaling
```bash
# Scale up the app service plan
az appservice plan update --name bihar-teacher-portal-plan --resource-group bihar-teacher-portal-rg --sku B1
```

## 🆘 Troubleshooting

### Common Issues

1. **Static Files Not Loading**
   - Verify wwwroot contains frontend files
   - Check web.config MIME types
   - Ensure UseStaticFiles() is called

2. **Angular Routing Issues**
   - Verify MapFallbackToFile("index.html") is configured
   - Check web.config rewrite rules

3. **API Calls Failing**
   - Verify CORS configuration
   - Check API base URL in frontend
   - Ensure authentication is working

### Debugging Commands
```bash
# Check app logs
az webapp log download --resource-group bihar-teacher-portal-rg --name <your-app-name>

# SSH into the container (Linux App Service)
az webapp ssh --resource-group bihar-teacher-portal-rg --name <your-app-name>

# Check app settings
az webapp config appsettings list --resource-group bihar-teacher-portal-rg --name <your-app-name>
```

## 💰 Cost Optimization

### Free Tier Limitations
- 60 CPU minutes per day
- 1 GB storage
- Custom domains not supported
- Always On not available

### Upgrade Recommendations
For production use, consider upgrading to:
- **Basic B1**: $13.14/month - Custom domains, SSL
- **Standard S1**: $56.94/month - Auto-scaling, staging slots

## 🎯 Next Steps

1. **Deploy to Azure** using the provided scripts
2. **Test thoroughly** in the Azure environment
3. **Configure monitoring** and alerts
4. **Set up CI/CD** for automated deployments
5. **Consider database migration** for production scale

## 📞 Support

For deployment issues:
1. Check Azure Activity Log in the portal
2. Review application logs
3. Verify all configuration settings
4. Test locally with production settings

The Bihar Teacher Management Portal is now ready for Azure deployment with a unified architecture that serves both the API and the Angular frontend from a single endpoint!