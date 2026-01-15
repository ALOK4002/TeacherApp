# Azure Blob Storage and Email Services Setup Guide

This guide provides detailed step-by-step instructions for setting up Azure Blob Storage and Azure Communication Services (Email) for the Teacher Document Management System.

---

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Part 1: Azure Blob Storage Setup](#part-1-azure-blob-storage-setup)
3. [Part 2: Azure Communication Services (Email) Setup](#part-2-azure-communication-services-email-setup)
4. [Part 3: Configure Application](#part-3-configure-application)
5. [Part 4: Testing](#part-4-testing)
6. [Troubleshooting](#troubleshooting)

---

## Prerequisites

Before starting, ensure you have:
- An active Azure subscription (create free account at https://azure.microsoft.com/free/)
- Azure Portal access (https://portal.azure.com)
- A verified domain (for email service) OR use Azure subdomain
- Basic understanding of Azure services

**Estimated Time:** 30-45 minutes  
**Cost:** Free tier available for both services (pay-as-you-go for production)

---

## Part 1: Azure Blob Storage Setup

### Step 1: Create a Storage Account

1. **Login to Azure Portal**
   - Go to https://portal.azure.com
   - Sign in with your Azure account

2. **Navigate to Storage Accounts**
   - Click on "Create a resource" (+ icon in top-left)
   - Search for "Storage account"
   - Click "Create"

3. **Configure Basic Settings**
   - **Subscription:** Select your Azure subscription
   - **Resource Group:** 
     - Click "Create new"
     - Name it: `rg-teacher-portal` (or any name you prefer)
     - Click "OK"
   - **Storage account name:** Enter a unique name (e.g., `teacherdocsstorage`)
     - Must be 3-24 characters
     - Only lowercase letters and numbers
     - Must be globally unique
   - **Region:** Select closest region (e.g., `Central India`, `East US`)
   - **Performance:** Standard (recommended for documents)
   - **Redundancy:** 
     - Development: `Locally-redundant storage (LRS)` - cheapest
     - Production: `Geo-redundant storage (GRS)` - recommended

4. **Configure Advanced Settings**
   - Click "Next: Advanced"
   - **Security:**
     - Enable "Require secure transfer for REST API operations" ✓
     - Minimum TLS version: `Version 1.2`
     - Enable "Allow Blob public access" ✓ (for document downloads)
   - **Blob storage:**
     - Access tier: `Hot` (for frequently accessed documents)
   - Leave other settings as default

5. **Configure Networking**
   - Click "Next: Networking"
   - **Network connectivity:** 
     - Select "Enable public access from all networks" (for development)
     - For production, configure firewall rules as needed

6. **Review and Create**
   - Click "Next: Data protection" (leave defaults)
   - Click "Next: Encryption" (leave defaults)
   - Click "Next: Tags" (optional - add tags if needed)
   - Click "Review + create"
   - Review all settings
   - Click "Create"
   - Wait 1-2 minutes for deployment to complete

### Step 2: Create a Blob Container

1. **Navigate to Your Storage Account**
   - After deployment completes, click "Go to resource"
   - OR search for your storage account name in the search bar

2. **Create Container**
   - In the left menu, under "Data storage", click "Containers"
   - Click "+ Container" at the top
   - **Name:** `teacher-documents` (lowercase, no spaces)
   - **Public access level:** 
     - Select "Blob (anonymous read access for blobs only)"
     - This allows documents to be downloaded via URL
   - Click "Create"

### Step 3: Get Connection String

1. **Access Keys**
   - In your storage account, click "Access keys" in the left menu (under "Security + networking")
   - You'll see two keys: `key1` and `key2`

2. **Copy Connection String**
   - Under "key1", click "Show" next to "Connection string"
   - Click the copy icon to copy the entire connection string
   - It looks like:
     ```
     DefaultEndpointsProtocol=https;AccountName=teacherdocsstorage;AccountKey=xxxxx;EndpointSuffix=core.windows.net
     ```
   - **IMPORTANT:** Save this securely - you'll need it for configuration

3. **Alternative: Copy Individual Values**
   - **Storage Account Name:** Your storage account name (e.g., `teacherdocsstorage`)
   - **Account Key:** Copy the key1 value
   - **Container Name:** `teacher-documents`

### Step 4: Verify Storage Setup

1. **Test Upload (Optional)**
   - Go to "Containers" → Click "teacher-documents"
   - Click "Upload"
   - Select any test file
   - Click "Upload"
   - Verify file appears in the list

2. **Get Blob URL Format**
   - Your blob URLs will be:
     ```
     https://{storage-account-name}.blob.core.windows.net/teacher-documents/{filename}
     ```

---

## Part 2: Azure Communication Services (Email) Setup

### Step 1: Create Communication Services Resource

1. **Navigate to Communication Services**
   - In Azure Portal, click "Create a resource"
   - Search for "Communication Services"
   - Click "Create"

2. **Configure Basic Settings**
   - **Subscription:** Select your Azure subscription
   - **Resource Group:** Select `rg-teacher-portal` (same as storage)
   - **Resource name:** Enter a name (e.g., `teacher-portal-communication`)
     - 1-63 characters
     - Letters, numbers, and hyphens
   - **Data location:** Select region (e.g., `India`, `United States`)
   - Click "Review + create"
   - Click "Create"
   - Wait for deployment (1-2 minutes)

3. **Get Connection String**
   - After deployment, click "Go to resource"
   - In the left menu, click "Keys" (under "Settings")
   - Copy the "Connection string" (Primary or Secondary)
   - It looks like:
     ```
     endpoint=https://teacher-portal-communication.communication.azure.com/;accesskey=xxxxx
     ```
   - **IMPORTANT:** Save this securely

### Step 2: Create Email Communication Service

1. **Create Email Service**
   - In Azure Portal, click "Create a resource"
   - Search for "Email Communication Services"
   - Click "Create"

2. **Configure Email Service**
   - **Subscription:** Select your Azure subscription
   - **Resource Group:** Select `rg-teacher-portal`
   - **Resource name:** Enter a name (e.g., `teacher-portal-email`)
   - **Region:** Select region (same as Communication Services)
   - **Data location:** Select data location
   - Click "Review + create"
   - Click "Create"
   - Wait for deployment

### Step 3: Configure Email Domain

You have two options:

#### Option A: Use Azure Managed Domain (Easiest - Recommended for Development)

1. **Provision Azure Subdomain**
   - Go to your Email Communication Service resource
   - In the left menu, click "Provision Domains"
   - Click "Add domain"
   - Select "Azure subdomain"
   - Click "Add"
   - Wait 2-3 minutes for provisioning
   - You'll get a domain like: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.azurecomm.net`

2. **Configure MailFrom Address**
   - After provisioning, click on the domain
   - Under "MailFrom addresses", click "Add"
   - **MailFrom address:** Enter a name (e.g., `noreply` or `documents`)
   - Full address will be: `noreply@xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.azurecomm.net`
   - Click "Add"

#### Option B: Use Custom Domain (For Production)

1. **Add Custom Domain**
   - Go to your Email Communication Service resource
   - Click "Provision Domains" → "Add domain"
   - Select "Custom domain"
   - Enter your domain (e.g., `teacherportal.com`)
   - Click "Add"

2. **Verify Domain Ownership**
   - Azure will provide DNS records (TXT, SPF, DKIM)
   - Go to your domain registrar (GoDaddy, Namecheap, etc.)
   - Add the DNS records provided by Azure
   - Wait 15-30 minutes for DNS propagation
   - Return to Azure Portal and click "Verify"

3. **Configure MailFrom Address**
   - After verification, click on your domain
   - Under "MailFrom addresses", click "Add"
   - Enter address (e.g., `noreply@teacherportal.com`)
   - Click "Add"

### Step 4: Connect Email Service to Communication Service

1. **Link Services**
   - Go to your Email Communication Service resource
   - In the left menu, click "Connected resources"
   - Click "Connect"
   - Select your Communication Services resource (`teacher-portal-communication`)
   - Click "Connect"

2. **Verify Connection**
   - You should see your Communication Service listed
   - Status should be "Connected"

### Step 5: Get Email Configuration Details

You need these values for your application:

1. **Communication Services Connection String**
   - From Communication Services → Keys
   - Copy the connection string

2. **Sender Email Address**
   - From Email Communication Service → Provision Domains
   - Click on your domain
   - Copy the MailFrom address (e.g., `noreply@xxxxxxxx.azurecomm.net`)

---

## Part 3: Configure Application

### Step 1: Update appsettings.json

1. **Open Configuration File**
   - Navigate to: `Backend/WebAPI/appsettings.json`

2. **Update Azure Storage Settings**
   ```json
   "AzureStorage": {
     "ConnectionString": "YOUR_STORAGE_CONNECTION_STRING_HERE",
     "ContainerName": "teacher-documents"
   }
   ```
   - Replace `YOUR_STORAGE_CONNECTION_STRING_HERE` with the connection string from Part 1, Step 3

3. **Update Azure Email Settings**
   ```json
   "AzureEmail": {
     "ConnectionString": "YOUR_COMMUNICATION_SERVICES_CONNECTION_STRING_HERE",
     "SenderEmail": "YOUR_SENDER_EMAIL_HERE"
   }
   ```
   - Replace `YOUR_COMMUNICATION_SERVICES_CONNECTION_STRING_HERE` with connection string from Part 2, Step 1
   - Replace `YOUR_SENDER_EMAIL_HERE` with MailFrom address from Part 2, Step 3

### Step 2: Example Configuration

Here's what your `appsettings.json` should look like:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=teacherportal.db"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-min-32-chars-long-12345678",
    "Issuer": "TeacherPortalAPI",
    "Audience": "TeacherPortalClient",
    "ExpiryInMinutes": 60
  },
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=teacherdocsstorage;AccountKey=abc123xyz789==;EndpointSuffix=core.windows.net",
    "ContainerName": "teacher-documents"
  },
  "AzureEmail": {
    "ConnectionString": "endpoint=https://teacher-portal-communication.communication.azure.com/;accesskey=xyz789abc123==",
    "SenderEmail": "noreply@12345678-1234-1234-1234-123456789012.azurecomm.net"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Step 3: Restart Application

1. **Stop Backend**
   - If running, stop the backend application (Ctrl+C)

2. **Start Backend**
   ```bash
   cd Backend
   dotnet run --project WebAPI
   ```

3. **Verify Configuration**
   - Check console output for any configuration errors
   - Backend should start on http://localhost:5162

---

## Part 4: Testing

### Test 1: Upload Document

1. **Navigate to Teacher Management**
   - Go to http://localhost:4200/teachers
   - Login if needed

2. **Open Documents Page**
   - Click "📄 Documents" button for any teacher

3. **Upload a Test Document**
   - Click "Choose File" and select a PDF or image
   - Select document type (e.g., "Resume")
   - Add remarks (optional)
   - Click "Upload Document"
   - Wait for success message

4. **Verify in Azure Portal**
   - Go to Azure Portal → Your Storage Account
   - Navigate to Containers → teacher-documents
   - You should see the uploaded file

### Test 2: Download Document

1. **In Documents Page**
   - Click "Download" button on any document
   - File should download to your computer

2. **Verify Blob URL**
   - Right-click "Download" and copy link
   - URL should be: `https://teacherdocsstorage.blob.core.windows.net/teacher-documents/filename`

### Test 3: Send Email

1. **In Documents Page**
   - Click "Send Email" button on any document
   - Fill in the form:
     - **Recipient Email:** Your email address
     - **Recipient Name:** Your name
     - **Message:** Test message
   - Click "Send Email"
   - Wait for success message

2. **Check Email**
   - Check your inbox (may take 1-2 minutes)
   - Check spam folder if not in inbox
   - Email should contain document link

### Test 4: Delete Document

1. **In Documents Page**
   - Click "Delete" button on any document
   - Confirm deletion
   - Document should be removed from list

2. **Verify in Azure Portal**
   - Go to Azure Portal → Storage Account → Containers
   - File should be deleted from blob storage

---

## Troubleshooting

### Issue 1: "Connection string is invalid"

**Symptoms:** Error when uploading documents

**Solutions:**
1. Verify connection string format in `appsettings.json`
2. Ensure no extra spaces or line breaks
3. Check that AccountKey is complete (ends with `==`)
4. Regenerate access key in Azure Portal if needed

### Issue 2: "Container not found"

**Symptoms:** 404 error when uploading

**Solutions:**
1. Verify container name is exactly `teacher-documents` (lowercase)
2. Check container exists in Azure Portal
3. Ensure container has "Blob" public access level

### Issue 3: "Email sending failed"

**Symptoms:** Error when sending email

**Solutions:**
1. Verify Communication Services connection string
2. Check sender email address is correct
3. Ensure Email Service is connected to Communication Service
4. Verify domain is provisioned and active
5. Check recipient email is valid

### Issue 4: "Access denied" when downloading

**Symptoms:** 403 error when clicking download

**Solutions:**
1. Verify container public access level is "Blob"
2. Check "Allow Blob public access" is enabled on storage account
3. Verify blob URL is correct

### Issue 5: Email not received

**Symptoms:** Email sent successfully but not received

**Solutions:**
1. Check spam/junk folder
2. Wait 2-5 minutes (email delivery can be delayed)
3. Verify sender email is from provisioned domain
4. Check Azure Communication Services logs in portal
5. For custom domains, verify DNS records are correct

### Issue 6: "Storage account not found"

**Symptoms:** Error on application startup

**Solutions:**
1. Verify storage account name in connection string
2. Check storage account exists in Azure Portal
3. Ensure storage account is in same subscription
4. Verify connection string is in correct format

---

## Cost Estimation

### Azure Blob Storage
- **Free Tier:** First 5 GB free
- **Storage:** ~$0.018 per GB/month (Hot tier)
- **Operations:** ~$0.004 per 10,000 operations
- **Estimated for 1000 documents (100 MB):** ~$0.50/month

### Azure Communication Services (Email)
- **Free Tier:** 100 emails/month free
- **Paid:** $0.0025 per email (after free tier)
- **Estimated for 500 emails/month:** ~$1.00/month

**Total Estimated Cost:** ~$1.50/month for moderate usage

---

## Security Best Practices

### 1. Connection Strings
- ✓ Never commit connection strings to Git
- ✓ Use Azure Key Vault for production
- ✓ Rotate access keys regularly (every 90 days)
- ✓ Use separate storage accounts for dev/prod

### 2. Access Control
- ✓ Use Managed Identities in production (no connection strings)
- ✓ Implement Azure AD authentication
- ✓ Configure firewall rules to restrict access
- ✓ Enable storage account encryption

### 3. Email Security
- ✓ Implement rate limiting for email sending
- ✓ Validate recipient email addresses
- ✓ Add SPF, DKIM, DMARC records for custom domains
- ✓ Monitor email sending logs

### 4. Document Security
- ✓ Scan uploaded files for viruses
- ✓ Validate file types and sizes
- ✓ Use SAS tokens for temporary access (instead of public blobs)
- ✓ Implement document access logging

---

## Production Deployment Checklist

Before deploying to production:

- [ ] Use custom domain for email (not Azure subdomain)
- [ ] Configure DNS records (SPF, DKIM, DMARC)
- [ ] Enable storage account firewall
- [ ] Use Geo-redundant storage (GRS)
- [ ] Implement Azure Key Vault for secrets
- [ ] Enable storage account logging
- [ ] Set up monitoring and alerts
- [ ] Configure backup and retention policies
- [ ] Test disaster recovery procedures
- [ ] Document all configurations
- [ ] Set up cost alerts
- [ ] Review security recommendations

---

## Additional Resources

### Documentation
- Azure Blob Storage: https://docs.microsoft.com/azure/storage/blobs/
- Azure Communication Services: https://docs.microsoft.com/azure/communication-services/
- Email Service: https://docs.microsoft.com/azure/communication-services/concepts/email/

### Support
- Azure Support: https://azure.microsoft.com/support/
- Community Forums: https://docs.microsoft.com/answers/
- Stack Overflow: Tag `azure-storage` or `azure-communication-services`

### Tools
- Azure Storage Explorer: https://azure.microsoft.com/features/storage-explorer/
- Azure CLI: https://docs.microsoft.com/cli/azure/
- Azure PowerShell: https://docs.microsoft.com/powershell/azure/

---

## Summary

You have successfully configured:
1. ✓ Azure Blob Storage for document storage
2. ✓ Azure Communication Services for email
3. ✓ Application configuration
4. ✓ Tested all functionality

Your Teacher Document Management System is now ready to use!

**Next Steps:**
1. Test with real teacher data
2. Configure production environment
3. Set up monitoring and alerts
4. Plan for scaling and backup

For questions or issues, refer to the Troubleshooting section or Azure documentation.
