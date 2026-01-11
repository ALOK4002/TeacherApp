#!/bin/bash

# Azure Deployment Script for Bihar Teacher Portal
# Make sure you have Azure CLI installed and are logged in

# Variables
RESOURCE_GROUP="bihar-teacher-portal-rg"
APP_SERVICE_PLAN="bihar-teacher-portal-plan"
WEB_APP_NAME="bihar-teacher-portal-$(date +%s)"  # Unique name with timestamp
LOCATION="East US"
SKU="F1"  # Free tier

echo "🚀 Starting Azure deployment for Bihar Teacher Portal..."

# Create Resource Group
echo "📦 Creating resource group: $RESOURCE_GROUP"
az group create --name $RESOURCE_GROUP --location "$LOCATION"

# Create App Service Plan
echo "📋 Creating app service plan: $APP_SERVICE_PLAN"
az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --sku $SKU --is-linux

# Create Web App
echo "🌐 Creating web app: $WEB_APP_NAME"
az webapp create --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --name $WEB_APP_NAME --runtime "DOTNETCORE|8.0"

# Configure app settings
echo "⚙️ Configuring app settings..."
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME --settings \
    "JwtSettings__SecretKey=YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters" \
    "JwtSettings__Issuer=BiharTeacherPortal" \
    "JwtSettings__Audience=BiharTeacherPortalUsers" \
    "ConnectionStrings__DefaultConnection=Data Source=app.db"

# Enable logging
echo "📝 Enabling application logging..."
az webapp log config --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME --application-logging filesystem --level information

echo "✅ Azure resources created successfully!"
echo "🌐 Web App URL: https://$WEB_APP_NAME.azurewebsites.net"
echo ""
echo "📋 Next steps:"
echo "1. Build and deploy your application:"
echo "   cd Backend/WebAPI"
echo "   dotnet publish -c Release -o ./publish"
echo "   az webapp deployment source config-zip --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME --src ./publish.zip"
echo ""
echo "2. Or use the deployment script: ./deploy-app.sh $WEB_APP_NAME"