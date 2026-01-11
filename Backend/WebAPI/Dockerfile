# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js for Angular build
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

# Copy frontend and build it
COPY Frontend/ Frontend/
WORKDIR /src/Frontend
RUN npm install
RUN npm run build

# Copy backend projects
WORKDIR /src
COPY Backend/Domain/ Backend/Domain/
COPY Backend/Application/ Backend/Application/
COPY Backend/Infrastructure/ Backend/Infrastructure/
COPY Backend/WebAPI/ Backend/WebAPI/

# Copy frontend build to backend wwwroot
RUN mkdir -p Backend/WebAPI/wwwroot
RUN cp -r Frontend/dist/Frontend/* Backend/WebAPI/wwwroot/

# Restore and build backend
WORKDIR /src/Backend/WebAPI
RUN dotnet restore "WebAPI.csproj"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create directory for SQLite database
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/app.db"

ENTRYPOINT ["dotnet", "WebAPI.dll"]