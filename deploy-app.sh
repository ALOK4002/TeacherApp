#!/bin/bash

# Application Deployment Script
# Usage: ./deploy-app.sh <web-app-name>

if [ -z "$1" ]; then
    echo "❌ Error: Please provide the web app name"
    echo "Usage: ./deploy-app.sh <web-app-name>"
    exit 1
fi

WEB_APP_NAME=$1
RESOURCE_GROUP="bihar-teacher-portal-rg"

echo "🚀 Deploying Bihar Teacher Portal to Azure..."
echo "📱 Web App: $WEB_APP_NAME"

# Step 1: Build Frontend
echo "🎨 Building Angular frontend..."
cd Frontend
npm install
npm run build
cd ..

# Step 2: Copy Frontend to Backend
echo "📁 Copying frontend to backend wwwroot..."
mkdir -p Backend/WebAPI/wwwroot
cp -r Frontend/dist/Frontend/* Backend/WebAPI/wwwroot/

# Step 3: Build and Publish Backend
echo "🔧 Building and publishing .NET backend..."
cd Backend/WebAPI
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o ./publish

# Step 4: Create deployment package
echo "📦 Creating deployment package..."
cd publish
zip -r ../deploy.zip .
cd ..

# Step 5: Deploy to Azure
echo "☁️ Deploying to Azure App Service..."
az webapp deployment source config-zip \
    --resource-group $RESOURCE_GROUP \
    --name $WEB_APP_NAME \
    --src deploy.zip

# Step 6: Restart the app
echo "🔄 Restarting web app..."
az webapp restart --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME

echo "✅ Deployment completed successfully!"
echo "🌐 Your app is available at: https://$WEB_APP_NAME.azurewebsites.net"
echo ""
echo "📋 Useful commands:"
echo "• View logs: az webapp log tail --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME"
echo "• Open in browser: az webapp browse --resource-group $RESOURCE_GROUP --name $WEB_APP_NAME"

# Cleanup
rm -f deploy.zip
cd ../..