# 📄 How to Access Teacher Documents

This guide shows you exactly where to find and use the document management features.

---

## 🎯 Quick Answer

**Where is the Documents button?**

The "📄 Documents" button is in the **Actions column** of the Teacher Management table, next to the Edit and Activate/Deactivate buttons.

---

## 📍 Step-by-Step Navigation

### Step 1: Login to the Application

1. Open your browser
2. Go to: `http://localhost:4200`
3. If not logged in, you'll see the login page
4. Enter your credentials and click "Login"

### Step 2: Navigate to Teacher Management

After login, you have two options:

**Option A**: Direct URL
- Go to: `http://localhost:4200/teachers`

**Option B**: From Welcome Page
- Click "Teacher Management" button

### Step 3: Find the Documents Button

In the Teacher Management page, you'll see:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Teacher Management System                                           │
│  [Add Teacher] [Teacher Report] [Notice Board] [About] [Logout]     │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│  Filters:                                                             │
│  District: [All Districts ▼]  Search: [____________]                │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│ Name    │ District │ School │ ... │ Actions                         │
├─────────┼──────────┼────────┼─────┼─────────────────────────────────┤
│ Rajesh  │ Patna    │ DPS    │ ... │ [📄 Documents] [Edit] [Deact.] │
│ Kumar   │          │        │     │      ↑                          │
├─────────┼──────────┼────────┼─────┤      │                          │
│ Priya   │ Gaya     │ DAV    │ ... │ [📄 Documents] [Edit] [Deact.] │
│ Singh   │          │        │     │  CLICK HERE!                    │
├─────────┼──────────┼────────┼─────┤                                 │
│ Amit    │ Patna    │ St.    │ ... │ [📄 Documents] [Edit] [Deact.] │
│ Sharma  │          │ Xavier │     │                                 │
└─────────┴──────────┴────────┴─────┴─────────────────────────────────┘
```

**Look for**: Purple button with 📄 icon and "Documents" text

### Step 4: Click Documents Button

1. Find any teacher in the table
2. Look at the rightmost column (Actions)
3. Click the **"📄 Documents"** button (purple color)
4. You'll be taken to the document management page for that teacher

---

## 📋 Document Management Page

After clicking "📄 Documents", you'll see:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Teacher Documents - [Teacher Name]                                  │
│  [← Back to Teachers]                                                │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│  Upload New Document                                                  │
│                                                                       │
│  Choose File: [Browse...]                                            │
│  Document Type: [Resume ▼]                                           │
│  Remarks: [_________________________________]                        │
│  [Upload Document]                                                   │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│  Uploaded Documents (5)                                               │
│                                                                       │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ 📄 Resume - resume_2024.pdf                                  │   │
│  │ Uploaded: Jan 15, 2026 | Size: 2.5 MB                       │   │
│  │ Remarks: Updated resume for 2024                            │   │
│  │ [Download] [Send Email] [Delete]                            │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                       │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ 📄 Matric - matric_certificate.jpg                          │   │
│  │ Uploaded: Jan 10, 2026 | Size: 1.2 MB                       │   │
│  │ Remarks: 10th standard certificate                          │   │
│  │ [Download] [Send Email] [Delete]                            │   │
│  └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 🎨 Visual Identification

### Button Appearance

The Documents button has these characteristics:

**Color**: Purple background (`#6f42c1`)  
**Icon**: 📄 (document emoji)  
**Text**: "Documents"  
**Size**: Same as Edit button  
**Position**: First button in Actions column (before Edit)

### Button States

**Normal**: Purple background, white text  
**Hover**: Darker purple (`#5a32a3`)  
**Click**: Navigates to document page

---

## 🔍 Troubleshooting

### Issue: Can't see Documents button

**Possible Causes**:

1. **Frontend not updated**
   - Solution: Refresh browser (Ctrl+F5 or Cmd+Shift+R)
   - Check if frontend is running on port 4200

2. **CSS not loaded**
   - Solution: Clear browser cache
   - Hard refresh the page

3. **Not on Teacher Management page**
   - Solution: Navigate to http://localhost:4200/teachers

4. **Button hidden by screen size**
   - Solution: Scroll right in the table
   - Try on larger screen or zoom out

### Issue: Button visible but not clickable

**Possible Causes**:

1. **JavaScript error**
   - Solution: Open browser console (F12)
   - Check for errors
   - Refresh page

2. **Route not configured**
   - Solution: Check that frontend is running
   - Verify no console errors

### Issue: Button clicks but nothing happens

**Possible Causes**:

1. **Route not found**
   - Solution: Check browser console for routing errors
   - Verify frontend is running

2. **Teacher ID missing**
   - Solution: Check that teacher has valid ID
   - Refresh teacher list

---

## 📱 Mobile/Tablet View

On smaller screens:

1. Table may be scrollable horizontally
2. Swipe left to see Actions column
3. Documents button may appear smaller
4. All functionality remains the same

---

## 🎯 Quick Access URLs

### Direct URLs (after login):

**Teacher Management**:
```
http://localhost:4200/teachers
```

**Specific Teacher Documents** (replace {id} with teacher ID):
```
http://localhost:4200/teacher-documents/1
http://localhost:4200/teacher-documents/2
http://localhost:4200/teacher-documents/3
```

---

## 📊 Feature Overview

### What You Can Do

From the Teacher Management page:
- ✅ View all teachers
- ✅ Click Documents button for any teacher
- ✅ Navigate to document management

From the Document Management page:
- ✅ Upload new documents
- ✅ View all documents for that teacher
- ✅ Download documents
- ✅ Send documents via email
- ✅ Delete documents
- ✅ Search and filter documents

---

## 🎓 Usage Tips

### Best Practices

1. **Upload documents immediately after adding a teacher**
   - Click Documents button right after creating teacher
   - Upload all required certificates

2. **Use descriptive remarks**
   - Add notes about document version
   - Include expiry dates if applicable

3. **Organize by document type**
   - Use predefined types (Resume, Matric, etc.)
   - Use "Other" for custom types

4. **Regular backups**
   - Download important documents periodically
   - Keep local copies of critical certificates

### Document Types Available

- **Resume**: Teacher's CV/Resume
- **Matric**: 10th standard certificate
- **Inter**: 12th standard certificate
- **Graduate**: Bachelor's degree
- **PG**: Post-graduate degree
- **Other**: Custom document type

---

## 🔐 Security Notes

### Access Control

- ✅ Must be logged in to access documents
- ✅ JWT token required for all operations
- ✅ User ID tracked for all uploads
- ✅ Secure Azure storage

### Privacy

- Documents stored securely in Azure
- Only authorized users can access
- Audit trail maintained
- Soft delete (documents not permanently removed)

---

## 📞 Need More Help?

### Documentation

- **Setup Guide**: [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)
- **Quick Reference**: [AZURE_QUICK_REFERENCE.md](AZURE_QUICK_REFERENCE.md)
- **Architecture**: [DOCUMENT_MANAGEMENT_ARCHITECTURE.md](DOCUMENT_MANAGEMENT_ARCHITECTURE.md)
- **Setup Checklist**: [AZURE_SETUP_CHECKLIST.md](AZURE_SETUP_CHECKLIST.md)

### Common Questions

**Q: How many documents can I upload per teacher?**  
A: Unlimited (subject to Azure storage limits)

**Q: What file types are supported?**  
A: PDF, JPG, PNG, DOCX, and more (configurable)

**Q: What's the maximum file size?**  
A: 10 MB per file (configurable in backend)

**Q: Can I upload multiple files at once?**  
A: Currently one at a time (bulk upload coming soon)

**Q: Are documents backed up?**  
A: Yes, in Azure Blob Storage with redundancy

---

## ✅ Quick Checklist

Before using document management:

- [ ] Backend running on port 5162
- [ ] Frontend running on port 4200
- [ ] Logged in to application
- [ ] On Teacher Management page
- [ ] Can see teacher table
- [ ] Can see Actions column
- [ ] Can see purple Documents button
- [ ] Azure services configured (for upload to work)

---

## 🎉 You're Ready!

Now you know:
- ✅ Where to find the Documents button
- ✅ How to navigate to document management
- ✅ What features are available
- ✅ How to troubleshoot common issues

**Start managing teacher documents now!**

---

**Last Updated**: January 2026  
**Version**: 1.0  
**Application**: Bihar Teacher Management Portal
