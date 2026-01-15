# 📤 Document Upload Feature - User Guide

## Overview

The Teacher Document Management system allows you to upload, view, download, and share teacher documents with a beautiful card-based interface.

---

## ✨ Features

### Upload Documents
- ✅ Multiple document types supported
- ✅ Custom document types
- ✅ Add remarks/notes
- ✅ Real-time upload progress
- ✅ **Success message** after upload
- ✅ **Error message** on failure

### View Documents
- ✅ **Card view** display
- ✅ Document type badges
- ✅ File size and upload date
- ✅ Remarks displayed
- ✅ Responsive grid layout

### Document Actions
- ✅ View in browser
- ✅ Download to computer
- ✅ Send via email
- ✅ Delete with confirmation

---

## 🎯 How to Use

### Step 1: Navigate to Documents Page

1. Go to Teacher Management page
2. Find the teacher in the table
3. Click the **"📄 Documents"** button (purple button)
4. You'll see the document management page

### Step 2: Upload a Document

1. **Select Document Type**
   - Choose from: Resume, Matric, Inter, Graduate, PG
   - Or select "Other" for custom type

2. **Add Custom Type** (if "Other" selected)
   - Enter your custom document type name

3. **Add Remarks** (optional)
   - Add notes about the document
   - Example: "Updated resume 2024", "Original certificate"

4. **Select File**
   - Click "Select File" button
   - Choose file from your computer
   - Supported: PDF, DOC, DOCX, JPG, JPEG, PNG
   - Max size: 10 MB

5. **Upload**
   - Click "📤 Upload Document" button
   - Wait for upload to complete
   - **Success message will appear**: "✅ Document uploaded successfully!"

### Step 3: View Uploaded Documents

After upload, documents appear as **cards** below the upload form:

```
┌─────────────────────────────────────┐
│ 📄  resume_2024.pdf                 │
│     [Resume]                        │
│                                     │
│ Size: 2.5 MB                        │
│ Uploaded: Jan 15, 2026, 3:30 PM    │
│ Remarks: Updated resume for 2024   │
│                                     │
│ [👁️ View] [⬇️ Download]            │
│ [✉️ Email] [🗑️ Delete]              │
└─────────────────────────────────────┘
```

---

## 📋 Success & Error Messages

### Success Messages

**After Upload:**
```
✅ Document "resume_2024.pdf" uploaded successfully!
```
- Appears at the top of the upload form
- Green background with checkmark
- Auto-disappears after 5 seconds
- Document appears in card view below

**After Delete:**
```
✅ Document "resume_2024.pdf" deleted successfully!
```
- Appears at the top of documents list
- Card is removed from view

### Error Messages

**Upload Failed:**
```
❌ Failed to upload document. Please try again.
```
- Appears at the top of the upload form
- Red background with X mark
- Shows specific error if available
- Auto-disappears after 10 seconds

**Common Error Reasons:**
- File too large (> 10 MB)
- Invalid file type
- Network connection issue
- Azure storage not configured
- Server error

**Download Failed:**
```
❌ Failed to download document. Please try again.
```

**Delete Failed:**
```
❌ Failed to delete document. Please try again.
```

---

## 🎨 Card View Layout

### Document Card Features

Each document is displayed as a card with:

1. **Header Section**
   - 📄 Document icon
   - File name
   - Document type badge (blue)
   - Custom type badge (purple, if applicable)

2. **Details Section**
   - File size (formatted: KB, MB)
   - Upload date and time
   - Remarks (if provided)

3. **Actions Section**
   - 👁️ View - Opens in new tab
   - ⬇️ Download - Downloads to computer
   - ✉️ Email - Send via email
   - 🗑️ Delete - Remove document

### Card Layout

**Desktop (3 columns):**
```
┌─────────┐ ┌─────────┐ ┌─────────┐
│ Card 1  │ │ Card 2  │ │ Card 3  │
└─────────┘ └─────────┘ └─────────┘
┌─────────┐ ┌─────────┐ ┌─────────┐
│ Card 4  │ │ Card 5  │ │ Card 6  │
└─────────┘ └─────────┘ └─────────┘
```

**Tablet (2 columns):**
```
┌─────────┐ ┌─────────┐
│ Card 1  │ │ Card 2  │
└─────────┘ └─────────┘
```

**Mobile (1 column):**
```
┌─────────┐
│ Card 1  │
└─────────┘
┌─────────┐
│ Card 2  │
└─────────┘
```

---

## 🎬 Complete Upload Flow

### Visual Flow

```
1. Click "📄 Documents" button
   ↓
2. Document page opens
   ↓
3. Fill upload form:
   - Select document type
   - Add custom type (if needed)
   - Add remarks (optional)
   - Choose file
   ↓
4. Click "📤 Upload Document"
   ↓
5. Button shows "⏳ Uploading..."
   ↓
6. Upload completes
   ↓
7. ✅ Success message appears
   ↓
8. Form clears automatically
   ↓
9. New document card appears below
   ↓
10. Success message fades after 5 seconds
```

### Error Flow

```
1. Upload fails
   ↓
2. ❌ Error message appears
   ↓
3. Form remains filled (can retry)
   ↓
4. Fix issue and retry
   ↓
5. Error message fades after 10 seconds
```

---

## 💡 Tips & Best Practices

### File Naming
- ✅ Use descriptive names: `resume_john_doe_2024.pdf`
- ✅ Avoid special characters: `certificate-matric.pdf`
- ❌ Avoid: `doc1.pdf`, `file.pdf`

### Document Types
- **Resume**: Teacher's CV or resume
- **Matric**: 10th standard certificate
- **Inter**: 12th standard certificate
- **Graduate**: Bachelor's degree
- **PG**: Master's or PhD degree
- **Other**: Any custom document type

### Remarks
- Add version info: "Updated Jan 2024"
- Add validity: "Valid until Dec 2025"
- Add notes: "Original certificate", "Attested copy"

### File Sizes
- Keep files under 5 MB when possible
- Compress large PDFs before upload
- Use JPEG instead of PNG for photos

---

## 🔧 Troubleshooting

### Issue: Upload button disabled

**Causes:**
- No file selected
- No document type selected
- "Other" selected but no custom type entered

**Solution:**
- Fill all required fields (marked with *)

### Issue: Upload fails immediately

**Causes:**
- File too large (> 10 MB)
- Invalid file type
- Network disconnected

**Solution:**
- Check file size and type
- Check internet connection
- Try again

### Issue: Upload hangs at "Uploading..."

**Causes:**
- Large file size
- Slow internet connection
- Server timeout

**Solution:**
- Wait a bit longer (large files take time)
- Check network connection
- Refresh page and try again

### Issue: Success message but no card appears

**Causes:**
- Page not refreshed
- Filter applied
- Display issue

**Solution:**
- Scroll down to see cards
- Refresh the page
- Check browser console for errors

### Issue: Cards not displaying properly

**Causes:**
- Browser zoom level
- Small screen size
- CSS not loaded

**Solution:**
- Reset browser zoom (Ctrl+0)
- Try on larger screen
- Hard refresh (Ctrl+Shift+R)

---

## 📱 Mobile Experience

### Mobile-Optimized Features

1. **Responsive Layout**
   - Single column card view
   - Full-width buttons
   - Touch-friendly controls

2. **File Selection**
   - Native file picker
   - Camera option (for photos)
   - Gallery access

3. **Actions**
   - Stacked buttons (easier to tap)
   - Larger touch targets
   - Swipe-friendly cards

---

## 🎨 Visual Design

### Color Coding

- **Blue badges**: Standard document types
- **Purple badges**: Custom document types
- **Green buttons**: Download actions
- **Orange buttons**: Email actions
- **Red buttons**: Delete actions
- **Cyan buttons**: View actions

### Animations

- **Card hover**: Slight lift effect
- **Message appear**: Slide-in animation
- **Button hover**: Color change
- **Upload progress**: Loading spinner

---

## 📊 Document Statistics

The page header shows:
```
📋 Uploaded Documents (5)
```

This count updates automatically when:
- Documents are uploaded
- Documents are deleted
- Page is refreshed

---

## 🔐 Security Features

### Access Control
- Must be logged in
- JWT token required
- User ID tracked

### File Validation
- File type checking
- Size limit enforcement
- Secure file naming

### Storage
- Azure Blob Storage
- Encrypted at rest
- HTTPS only

---

## ✅ Feature Checklist

After implementation, verify:

- [ ] Upload form visible
- [ ] All document types available
- [ ] Custom type field appears for "Other"
- [ ] File selection works
- [ ] Upload button enables when ready
- [ ] Upload shows progress
- [ ] **Success message appears after upload**
- [ ] **Error message appears on failure**
- [ ] **Documents display as cards**
- [ ] Cards show all information
- [ ] All action buttons work
- [ ] Cards are responsive
- [ ] Mobile view works properly

---

## 📞 Support

### Common Questions

**Q: How many documents can I upload?**
A: Unlimited (subject to Azure storage limits)

**Q: Can I upload multiple files at once?**
A: Currently one at a time (bulk upload coming soon)

**Q: Can I edit document details after upload?**
A: Not yet (feature planned)

**Q: Can I replace a document?**
A: Delete old one and upload new one

**Q: Are documents backed up?**
A: Yes, in Azure Blob Storage with redundancy

---

## 🚀 Next Steps

After uploading documents:

1. **Download** - Test download functionality
2. **Email** - Try sending via email
3. **View** - Open in browser to verify
4. **Organize** - Use consistent naming and types

---

**Ready to upload?** Go to Teacher Management → Click "📄 Documents" → Start uploading!

**Need Azure setup?** See [AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md](AZURE_BLOB_STORAGE_EMAIL_SETUP_GUIDE.md)
