#!/bin/bash

# Build Script for Manual Azure Deployment
# This script prepares the application for manual deployment

echo "🚀 Building Bihar Teacher Portal for Azure Deployment..."

# Step 1: Build Frontend
echo "🎨 Building Angular Frontend..."
cd Frontend
npm install
npm run build
cd ..

# Step 2: Ensure Database Exists
echo "🗄️ Ensuring SQLite Database Exists..."
cd Backend/WebAPI

# Check if database exists, if not create it
if [ ! -f "authapp.db" ]; then
    echo "📊 Creating SQLite database..."
    dotnet ef database update
else
    echo "✅ Database already exists: authapp.db"
fi

# Step 3: Build Backend
echo "🔧 Building .NET Backend..."
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o ./publish

# Step 4: Verify Database is in Publish Folder
echo "🔍 Verifying database inclusion..."
if [ -f "./publish/authapp.db" ]; then
    echo "✅ Database included in publish folder"
    echo "📊 Database size: $(du -h ./publish/authapp.db | cut -f1)"
else
    echo "⚠️ Database not found in publish folder, copying manually..."
    cp authapp.db ./publish/
    echo "✅ Database copied to publish folder"
fi

# Step 5: Verify Frontend Files are in Publish Folder
echo "🔍 Verifying frontend files..."
if [ -d "./publish/wwwroot/browser" ]; then
    echo "✅ Frontend files included in publish folder"
    echo "📁 Frontend files: $(ls -la ./publish/wwwroot/browser/ | wc -l) items"
else
    echo "⚠️ Frontend files not found, this might cause issues"
fi

# Step 6: Create Deployment Package
echo "📦 Creating Deployment Package..."
cd publish
zip -r ../deploy.zip .
cd ..

echo "✅ Build completed successfully!"
echo ""
echo "📋 Deployment package created: Backend/WebAPI/deploy.zip"
echo "📁 Package size: $(du -h deploy.zip | cut -f1)"
echo ""
echo "📊 Package contents:"
echo "$(unzip -l deploy.zip | head -20)"
echo ""
echo "🌐 Next steps for manual deployment:"
echo "1. Login to Azure Portal: https://portal.azure.com"
echo "2. Go to your Web App → Advanced Tools → Go"
echo "3. Navigate to site/wwwroot in Kudu console"
echo "4. Upload and extract deploy.zip"
echo ""
echo "📖 For detailed instructions, see: MANUAL_AZURE_DEPLOYMENT_GUIDE.md"

cd ../..