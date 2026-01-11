# Quick Manual Deployment Reference Card

## 🔧 Angular Configuration (Already Done)
- ✅ **Build Output**: Configured to build directly to `../Backend/WebAPI/wwwroot`
- ✅ **Command**: `cd Frontend && npm run build`

## 🌐 Azure Portal Steps (One-time Setup)

### 1. Create Resources
1. **Resource Group**: `bihar-teacher-portal-rg`
2. **App Service Plan**: `bihar-teacher-portal-plan` (F1 Free)
3. **Web App**: `bihar-teacher-portal-[unique-suffix]` (.NET 8, Linux)

### 2. Configure App Settings
Add these in Web App → Configuration → Application settings:
```
JwtSettings__SecretKey = YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters
JwtSettings__Issuer = BiharTeacherPortal
JwtSettings__Audience = BiharTeacherPortalUsers
ConnectionStrings__DefaultConnection = Data Source=authapp.db
ASPNETCORE_ENVIRONMENT = Production
```

## 📦 Build & Deploy Process

### Quick Build Command
```bash
./build-for-deployment.sh
```
*This script now automatically includes the SQLite database in the deployment package*

### Manual Build Steps
```bash
# 1. Build Frontend
cd Frontend && npm run build && cd ..

# 2. Build Backend  
cd Backend/WebAPI
dotnet publish -c Release -o ./publish

# 3. Ensure Database is Included
cp authapp.db ./publish/ # (if not automatically copied)

# 4. Create ZIP
cd publish && zip -r ../deploy.zip . && cd ..
```

### Deploy via Kudu (Easiest)
1. Go to: `https://your-app-name.scm.azurewebsites.net`
2. Debug Console → CMD
3. Navigate to: `site/wwwroot`
4. **Drag & drop** `deploy.zip`
5. Extract and delete ZIP

## 🔍 Quick Test
- **App URL**: `https://your-app-name.azurewebsites.net`
- **API Test**: `https://your-app-name.azurewebsites.net/api/auth/login`
- **Logs**: Web App → Log stream

## 🛠️ Troubleshooting
- **404 on routes**: Check `web.config` deployed
- **Static files 404**: Verify files in `wwwroot/browser/`
- **API errors**: Check CORS in `Program.cs`
- **Database issues**: Check connection string

## 📱 File Structure After Deployment
```
site/wwwroot/
├── browser/           # Angular app files
│   ├── index.html
│   ├── main-*.js
│   └── styles-*.css
├── authapp.db         # SQLite database (with pre-seeded data)
├── WebAPI.dll         # .NET application
├── web.config         # IIS configuration
└── appsettings.json   # App configuration
```

**📊 Database Information:**
- **File**: `authapp.db` (SQLite database)
- **Contains**: Pre-seeded Bihar schools, districts, pincodes
- **Size**: ~2-5 MB (depending on data)
- **Backup**: Automatically included in every deployment

## 🎯 Success Checklist
- [ ] Web app loads
- [ ] Registration works
- [ ] Login works  
- [ ] All routes work
- [ ] APIs respond
- [ ] Database works