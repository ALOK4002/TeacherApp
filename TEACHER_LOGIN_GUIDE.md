# 👨‍🏫 Teacher Login Guide - Complete Flow

## 🎯 Overview
Teachers need to be registered and approved by an admin before they can login. Here's the complete step-by-step process.

---

## 📝 Step 1: Register as Teacher

1. **Go to Registration Page**
   - URL: http://localhost:4200/register

2. **Fill in the Registration Form**
   - **Username**: `teacher1` (or any username you prefer)
   - **Email**: `teacher1@test.com` (or any email)
   - **Role**: Select **"Teacher"** from the dropdown
   - **Password**: `test123` (or any password)
   - **Confirm Password**: `test123` (same as password)

3. **Submit Registration**
   - Click "Create Account"
   - You'll see a success message: 
     > "Your account is pending admin approval. You will be notified once approved."

4. **Try to Login (Will Fail)**
   - If you try to login now, you'll see:
     > "Your account is pending admin approval. Please wait for approval before logging in."

---

## ✅ Step 2: Admin Approves Teacher

### 2.1 Login as Admin
1. **Go to Login Page**
   - URL: http://localhost:4200/login

2. **Enter Admin Credentials**
   - **Username**: `admin`
   - **Password**: `admin`

3. **Click "Sign In"**
   - You'll be redirected to Teacher Management page

### 2.2 Navigate to User Onboarding
1. **Option A: Direct URL**
   - Go to: http://localhost:4200/user-onboarding

2. **Option B: From Welcome Page**
   - If you're on the welcome page, click "👥 User Onboarding"

### 2.3 Approve the Teacher
1. **View Pending Users**
   - You'll see the "Pending Users" tab (default)
   - You should see `teacher1` in the list with:
     - Username: teacher1
     - Email: teacher1@test.com
     - Role: Teacher
     - Status: Pending (⏳)

2. **Approve the User**
   - Click the green **"✅ Approve"** button
   - You'll see a success message: "User approved successfully!"
   - The user will disappear from the pending list

3. **Verify Approval (Optional)**
   - Click the "👥 All Users" tab
   - You should see `teacher1` with Status: "Approved" (✅)

4. **Logout**
   - Click "🚪 Logout" button

---

## 🎓 Step 3: Teacher Login & Complete Profile

### 3.1 Login as Teacher
1. **Go to Login Page**
   - URL: http://localhost:4200/login

2. **Enter Teacher Credentials**
   - **Username**: `teacher1`
   - **Password**: `test123`

3. **Click "Sign In"**
   - Since this is the first login and no profile exists
   - You'll be automatically redirected to: **Self-Declaration Form**

### 3.2 Complete Self-Declaration Form
1. **Fill in Your Profile**
   - **Teacher Name**: `John Doe` (your full name)
   - **Email**: `teacher1@test.com` (pre-filled, read-only)
   - **Contact Number**: `9876543210` (10 digits)
   - **District**: Select from dropdown (e.g., `Patna`)
   - **Pincode**: Select from dropdown (auto-populated based on district)
   - **Address**: `123 Main Street, Patna, Bihar`
   - **School**: Select from dropdown (e.g., any Patna school)
   - **Class Teaching**: `Class 10`
   - **Subject**: `Mathematics`
   - **Qualification**: `B.Ed, M.Sc Mathematics`
   - **Date of Joining**: Select date (e.g., `2020-01-15`)

2. **Submit Profile**
   - Click "💾 Save Profile"
   - You'll see: "Profile created successfully! Redirecting to dashboard..."
   - You'll be redirected to: **User Dashboard**

---

## 🏠 Step 4: User Dashboard

After completing your profile, you'll see your **User Dashboard** with:

### Quick Stats
- 📄 **Documents Uploaded**: 0 (initially)
- ✅ **Profile Status**: Complete
- 🏫 **School**: Your selected school name

### Profile Summary
- All your profile information displayed in a card
- Button to "✏️ Edit Profile"

### Quick Actions
- **📄 My Documents** - Upload and manage your documents
- **✏️ Edit Profile** - Update your profile information
- **ℹ️ About Portal** - Learn about the portal

### Recent Documents
- Will show your uploaded documents (empty initially)

---

## 📄 Step 5: Upload Documents

1. **Navigate to My Documents**
   - From dashboard, click "📄 My Documents"
   - Or go directly to: http://localhost:4200/my-documents

2. **Upload a Document**
   - **Document Type**: Select from dropdown
     - Resume/CV
     - Matric Certificate
     - Intermediate Certificate
     - Graduate Degree
     - Post Graduate Degree
     - Other Document
   - **Custom Document Type**: (if "Other" selected)
   - **Remarks**: Optional notes about the document
   - **Select File**: Choose a file (PDF, DOC, DOCX, JPG, PNG)
   - Click "📤 Upload Document"

3. **View Your Documents**
   - Documents appear in card view
   - Each card shows:
     - Document type badge
     - File name
     - File size
     - Upload date
     - Remarks (if any)

4. **Document Actions**
   - **👁️ View** - Open document in new tab
   - **⬇️ Download** - Download to your computer
   - **📧 Email** - Send document via email
   - **🗑️ Delete** - Remove document

---

## 🔄 Subsequent Logins

After your first login and profile completion:

1. **Login as Teacher**
   - Username: `teacher1`
   - Password: `test123`

2. **Automatic Redirect**
   - Since you now have a profile
   - You'll be redirected directly to: **User Dashboard**

3. **Access Your Features**
   - View/Edit Profile
   - Upload/Manage Documents
   - View Notice Board
   - Access About Us page

---

## 🚫 What Teachers CANNOT Access

Teachers are restricted from:
- ❌ User Onboarding (Admin only)
- ❌ School Management (Admin only)
- ❌ Teacher Management (Admin only)
- ❌ Teacher Report (Admin only)
- ❌ Other teachers' documents (Admin only)

Teachers can ONLY:
- ✅ View/Edit their own profile
- ✅ Upload/Manage their own documents
- ✅ View Notice Board
- ✅ View About Us page

---

## 🔐 Security Features

### Profile Isolation
- Teachers can only see their own profile
- Teachers can only see their own documents
- No access to other teachers' data

### Document Privacy
- Documents are stored securely in Azure Blob Storage
- Only the teacher who uploaded can view/manage their documents
- Admin can see all documents for management purposes

### Approval Workflow
- New teacher registrations require admin approval
- Teachers cannot login until approved
- Admin can reject registrations with a reason

---

## 🆘 Troubleshooting

### "Pending approval" message when trying to login
- **Cause**: Admin hasn't approved your account yet
- **Solution**: Wait for admin approval or contact admin

### Redirected to Self-Declaration form every time
- **Cause**: Profile not completed or not saved properly
- **Solution**: Complete and save the profile form

### Cannot upload documents
- **Cause**: File size too large or invalid format
- **Solution**: Use files under 10MB in PDF, DOC, DOCX, JPG, or PNG format

### "Unauthorized" error
- **Cause**: Session expired or not logged in
- **Solution**: Logout and login again

---

## 📞 Quick Reference

### URLs
- **Register**: http://localhost:4200/register
- **Login**: http://localhost:4200/login
- **User Dashboard**: http://localhost:4200/user-dashboard
- **My Documents**: http://localhost:4200/my-documents
- **Self-Declaration**: http://localhost:4200/self-declaration

### Default Admin Credentials
- **Username**: `admin`
- **Password**: `admin`

### Test Teacher Credentials (After Registration & Approval)
- **Username**: `teacher1`
- **Password**: `test123`

---

## ✅ Success Checklist

- [ ] Registered as teacher
- [ ] Admin approved my account
- [ ] Logged in successfully
- [ ] Completed self-declaration form
- [ ] Viewed user dashboard
- [ ] Uploaded at least one document
- [ ] Viewed my documents
- [ ] Downloaded a document
- [ ] Edited my profile

---

**Happy Teaching! 🎓**
