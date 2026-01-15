# Azure Services Quick Reference Card

## 🚀 Quick Links

- **Full Setup Guide**: [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)
- **Azure Portal**: https://portal.azure.com
- **Azure Free Account**: https://azure.microsoft.com/free/

---

## 📦 What You Need

### 1. Azure Blob Storage
**Purpose**: Store teacher documents (PDFs, images, etc.)

**Quick Steps**:
1. Create Storage Account → Name: `teacherdocsstorage`
2. Create Container → Name: `teacher-documents`
3. Copy Connection String from "Access Keys"

**Time**: ~5 minutes  
**Cost**: ~$0.50/month for 1000 documents

### 2. Azure Communication Services (Email)
**Purpose**: Send documents via email

**Quick Steps**:
1. Create Communication Services → Name: `teacher-portal-communication`
2. Create Email Service → Name: `teacher-portal-email`
3. Provision Azure Subdomain (easiest option)
4. Copy Connection String and Sender Email

**Time**: ~10 minutes  
**Cost**: Free for first 100 emails/month, then $0.0025/email

---

## ⚙️ Configuration Template

Copy this into `Backend/WebAPI/appsettings.json`:

```json
{
  "AzureStorage": {
    "ConnectionString": "PASTE_YOUR_STORAGE_CONNECTION_STRING_HERE",
    "ContainerName": "teacher-documents"
  },
  "AzureEmail": {
    "ConnectionString": "PASTE_YOUR_COMMUNICATION_CONNECTION_STRING_HERE",
    "SenderEmail": "PASTE_YOUR_SENDER_EMAIL_HERE"
  }
}
```

### Example (with fake values):
```json
{
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=teacherdocsstorage;AccountKey=abc123xyz789==;EndpointSuffix=core.windows.net",
    "ContainerName": "teacher-documents"
  },
  "AzureEmail": {
    "ConnectionString": "endpoint=https://teacher-portal-communication.communication.azure.com/;accesskey=xyz789abc123==",
    "SenderEmail": "noreply@12345678-1234-1234-1234-123456789012.azurecomm.net"
  }
}
```

---

## 🔍 Where to Find Values

### Storage Connection String
1. Azure Portal → Storage Accounts
2. Click your storage account
3. Left menu → "Access keys"
4. Copy "Connection string" under key1

### Email Connection String
1. Azure Portal → Communication Services
2. Click your communication service
3. Left menu → "Keys"
4. Copy "Connection string" (Primary)

### Sender Email
1. Azure Portal → Email Communication Services
2. Click your email service
3. Left menu → "Provision Domains"
4. Click your domain
5. Copy the MailFrom address (e.g., `noreply@xxxxxxxx.azurecomm.net`)

---

## ✅ Testing Checklist

After configuration:

1. **Restart Backend**
   ```bash
   cd Backend
   dotnet run --project WebAPI
   ```

2. **Test Upload**
   - Go to http://localhost:4200/teachers
   - Click "📄 Documents" on any teacher
   - Upload a test file
   - ✓ Should see success message

3. **Test Download**
   - Click "Download" on uploaded document
   - ✓ File should download

4. **Test Email**
   - Click "Send Email" on any document
   - Enter your email address
   - ✓ Should receive email in 1-2 minutes

---

## 🆘 Common Issues

### "Connection string is invalid"
→ Check for extra spaces or line breaks in appsettings.json

### "Container not found"
→ Verify container name is exactly `teacher-documents` (lowercase)

### "Email sending failed"
→ Verify Email Service is connected to Communication Service in Azure Portal

### Email not received
→ Check spam folder, wait 2-5 minutes

---

## 💡 Tips

- **Development**: Use Azure Managed Domain (no DNS setup needed)
- **Production**: Use custom domain for professional emails
- **Security**: Never commit connection strings to Git
- **Cost**: Set up billing alerts in Azure Portal
- **Backup**: Enable soft delete on storage account

---

## 📚 Full Documentation

For complete step-by-step instructions with screenshots:
👉 **[AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)**

---

## 🎯 Total Setup Time

- Azure Blob Storage: ~5 minutes
- Azure Email Service: ~10 minutes
- Application Configuration: ~2 minutes
- Testing: ~5 minutes

**Total: ~20-25 minutes**

---

## 💰 Monthly Cost Estimate

| Service | Usage | Cost |
|---------|-------|------|
| Blob Storage | 1000 docs (100 MB) | $0.50 |
| Email Service | 500 emails | $1.00 |
| **Total** | | **$1.50/month** |

*First 100 emails/month are free*

---

**Need Help?** See the full guide: [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)
