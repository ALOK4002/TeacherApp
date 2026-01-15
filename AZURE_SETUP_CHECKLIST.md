# ✅ Azure Setup Checklist

Print this page or keep it open while setting up Azure services.

---

## 📦 Part 1: Azure Blob Storage (10 minutes)

### Step 1: Create Storage Account
- [ ] Login to Azure Portal (https://portal.azure.com)
- [ ] Click "Create a resource"
- [ ] Search for "Storage account"
- [ ] Click "Create"

### Step 2: Configure Storage Account
- [ ] Select your subscription
- [ ] Create/select resource group: `rg-teacher-portal`
- [ ] Enter storage account name: `teacherdocsstorage` (or your choice)
- [ ] Select region: `Central India` (or closest to you)
- [ ] Performance: `Standard`
- [ ] Redundancy: `LRS` (dev) or `GRS` (prod)
- [ ] Click "Review + create"
- [ ] Click "Create"
- [ ] Wait for deployment (1-2 minutes)

### Step 3: Create Container
- [ ] Click "Go to resource"
- [ ] Click "Containers" in left menu
- [ ] Click "+ Container"
- [ ] Name: `teacher-documents`
- [ ] Public access level: `Blob`
- [ ] Click "Create"

### Step 4: Get Connection String
- [ ] Click "Access keys" in left menu
- [ ] Click "Show" next to Connection string (key1)
- [ ] Click copy icon
- [ ] Paste in notepad/text editor
- [ ] Label it: "Storage Connection String"

**✅ Blob Storage Complete!**

---

## 📧 Part 2: Azure Communication Services (15 minutes)

### Step 1: Create Communication Services
- [ ] In Azure Portal, click "Create a resource"
- [ ] Search for "Communication Services"
- [ ] Click "Create"
- [ ] Select subscription
- [ ] Resource group: `rg-teacher-portal`
- [ ] Resource name: `teacher-portal-communication`
- [ ] Data location: `India` (or your region)
- [ ] Click "Review + create"
- [ ] Click "Create"
- [ ] Wait for deployment

### Step 2: Get Communication Connection String
- [ ] Click "Go to resource"
- [ ] Click "Keys" in left menu
- [ ] Copy "Connection string" (Primary)
- [ ] Paste in notepad/text editor
- [ ] Label it: "Communication Connection String"

### Step 3: Create Email Service
- [ ] Click "Create a resource"
- [ ] Search for "Email Communication Services"
- [ ] Click "Create"
- [ ] Select subscription
- [ ] Resource group: `rg-teacher-portal`
- [ ] Resource name: `teacher-portal-email`
- [ ] Region: Same as Communication Services
- [ ] Click "Review + create"
- [ ] Click "Create"
- [ ] Wait for deployment

### Step 4: Provision Domain
- [ ] Click "Go to resource"
- [ ] Click "Provision Domains" in left menu
- [ ] Click "Add domain"
- [ ] Select "Azure subdomain" (easiest)
- [ ] Click "Add"
- [ ] Wait 2-3 minutes for provisioning
- [ ] Domain will look like: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.azurecomm.net`

### Step 5: Configure MailFrom Address
- [ ] Click on the provisioned domain
- [ ] Click "MailFrom addresses"
- [ ] Click "Add"
- [ ] MailFrom address: `noreply` (or your choice)
- [ ] Click "Add"
- [ ] Full address will be: `noreply@xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.azurecomm.net`
- [ ] Copy this full email address
- [ ] Paste in notepad/text editor
- [ ] Label it: "Sender Email"

### Step 6: Connect Services
- [ ] In Email Service, click "Connected resources"
- [ ] Click "Connect"
- [ ] Select your Communication Services resource
- [ ] Click "Connect"
- [ ] Verify status shows "Connected"

**✅ Email Service Complete!**

---

## ⚙️ Part 3: Configure Application (5 minutes)

### Step 1: Open Configuration File
- [ ] Navigate to project folder
- [ ] Open: `Backend/WebAPI/appsettings.json`

### Step 2: Update Azure Storage Settings
- [ ] Find the `AzureStorage` section
- [ ] Replace `ConnectionString` with your Storage Connection String
- [ ] Verify `ContainerName` is `teacher-documents`

```json
"AzureStorage": {
  "ConnectionString": "PASTE_YOUR_STORAGE_CONNECTION_STRING_HERE",
  "ContainerName": "teacher-documents"
}
```

### Step 3: Update Azure Email Settings
- [ ] Find the `AzureEmail` section
- [ ] Replace `ConnectionString` with your Communication Connection String
- [ ] Replace `SenderEmail` with your Sender Email address

```json
"AzureEmail": {
  "ConnectionString": "PASTE_YOUR_COMMUNICATION_CONNECTION_STRING_HERE",
  "SenderEmail": "PASTE_YOUR_SENDER_EMAIL_HERE"
}
```

### Step 4: Save File
- [ ] Save `appsettings.json`
- [ ] Close file

**✅ Configuration Complete!**

---

## 🧪 Part 4: Testing (10 minutes)

### Step 1: Restart Backend
- [ ] Stop backend if running (Ctrl+C)
- [ ] Navigate to Backend folder
- [ ] Run: `dotnet run --project WebAPI`
- [ ] Verify no errors in console
- [ ] Backend should start on http://localhost:5162

### Step 2: Open Frontend
- [ ] Open browser
- [ ] Go to: http://localhost:4200
- [ ] Login if needed

### Step 3: Test Document Upload
- [ ] Navigate to Teacher Management page
- [ ] Find any teacher in the table
- [ ] Click "📄 Documents" button
- [ ] Click "Choose File"
- [ ] Select a test file (PDF, image, etc.)
- [ ] Select document type: "Resume"
- [ ] Add remarks: "Test upload"
- [ ] Click "Upload Document"
- [ ] Wait for success message
- [ ] Verify document appears in list

### Step 4: Verify in Azure Portal
- [ ] Go to Azure Portal
- [ ] Navigate to your Storage Account
- [ ] Click "Containers"
- [ ] Click "teacher-documents"
- [ ] Verify your uploaded file is there

### Step 5: Test Download
- [ ] In document list, click "Download"
- [ ] Verify file downloads to your computer
- [ ] Open file to verify it's correct

### Step 6: Test Email
- [ ] In document list, click "Send Email"
- [ ] Enter your email address
- [ ] Enter your name
- [ ] Add message: "Test email"
- [ ] Click "Send Email"
- [ ] Wait for success message
- [ ] Check your email inbox (may take 1-2 minutes)
- [ ] Check spam folder if not in inbox
- [ ] Verify email received with document link

### Step 7: Test Delete
- [ ] In document list, click "Delete"
- [ ] Confirm deletion
- [ ] Verify document removed from list
- [ ] Check Azure Portal to verify file deleted

**✅ All Tests Passed!**

---

## 📝 Configuration Summary

Fill this out as you go:

### Azure Storage Account
```
Storage Account Name: _________________________________
Container Name: teacher-documents
Connection String: ___________________________________
___________________________________________________
___________________________________________________
```

### Azure Communication Services
```
Communication Service Name: ___________________________
Connection String: ___________________________________
___________________________________________________
___________________________________________________
```

### Azure Email Service
```
Email Service Name: ___________________________________
Domain: ______________________________________________
Sender Email: ________________________________________
```

---

## 🎯 Success Criteria

You're done when:

- [x] Storage account created and configured
- [x] Container "teacher-documents" exists
- [x] Email service created and connected
- [x] Domain provisioned and MailFrom configured
- [x] appsettings.json updated with all connection strings
- [x] Backend starts without errors
- [x] Can upload documents successfully
- [x] Documents appear in Azure Portal
- [x] Can download documents
- [x] Can send emails successfully
- [x] Emails received in inbox
- [x] Can delete documents

---

## ⏱️ Time Tracking

| Task | Estimated | Actual |
|------|-----------|--------|
| Azure Blob Storage | 10 min | _____ |
| Azure Email Service | 15 min | _____ |
| Application Config | 5 min | _____ |
| Testing | 10 min | _____ |
| **Total** | **40 min** | **_____** |

---

## 🆘 Quick Troubleshooting

### Issue: Can't find "Create a resource"
**Solution**: Look for the "+" icon in top-left of Azure Portal

### Issue: Storage account name already taken
**Solution**: Try a different name (must be globally unique)

### Issue: Connection string doesn't work
**Solution**: 
1. Check for extra spaces or line breaks
2. Ensure you copied the entire string
3. Verify string starts with "DefaultEndpointsProtocol=https"

### Issue: Email not sending
**Solution**:
1. Verify Email Service is "Connected" to Communication Service
2. Check sender email is correct (includes full domain)
3. Wait 2-5 minutes for email delivery

### Issue: Upload fails
**Solution**:
1. Check backend console for errors
2. Verify container name is exactly "teacher-documents"
3. Ensure container has "Blob" public access

---

## 📞 Need Help?

**Full Documentation**: [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)

**Quick Reference**: [AZURE_QUICK_REFERENCE.md](AZURE_QUICK_REFERENCE.md)

**Architecture**: [DOCUMENT_MANAGEMENT_ARCHITECTURE.md](DOCUMENT_MANAGEMENT_ARCHITECTURE.md)

---

## ✅ Final Checklist

Before closing this document:

- [ ] All Azure resources created
- [ ] All connection strings saved securely
- [ ] Application configured
- [ ] All tests passed
- [ ] Documentation reviewed
- [ ] Ready for production deployment

---

**Congratulations! Your Azure setup is complete!** 🎉

**Date Completed**: _______________

**Completed By**: _______________

**Notes**: 
___________________________________________________
___________________________________________________
___________________________________________________
