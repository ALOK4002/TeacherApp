#!/bin/bash

# Container Deployment Script for Azure Container Instances
# This provides an alternative to App Service deployment

RESOURCE_GROUP="bihar-teacher-portal-rg"
CONTAINER_NAME="bihar-teacher-portal-container"
IMAGE_NAME="bihar-teacher-portal"
LOCATION="East US"

echo "🐳 Building and deploying container to Azure..."

# Build the Docker image
echo "🔨 Building Docker image..."
docker build -t $IMAGE_NAME -f Backend/WebAPI/Dockerfile .

# Create Azure Container Registry (optional, for production)
# ACR_NAME="biharteacherportalacr$(date +%s)"
# az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic --admin-enabled true

# For this demo, we'll use a simple container instance
echo "☁️ Creating Azure Container Instance..."
az container create \
    --resource-group $RESOURCE_GROUP \
    --name $CONTAINER_NAME \
    --image $IMAGE_NAME \
    --cpu 1 \
    --memory 1.5 \
    --ports 80 \
    --dns-name-label bihar-teacher-portal-$(date +%s) \
    --location "$LOCATION" \
    --environment-variables \
        ASPNETCORE_ENVIRONMENT=Production \
        JwtSettings__SecretKey=YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters \
        JwtSettings__Issuer=BiharTeacherPortal \
        JwtSettings__Audience=BiharTeacherPortalUsers

echo "✅ Container deployment completed!"
echo "🌐 Your app will be available at the DNS name shown above"