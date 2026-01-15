# 🚀 Quick Start Guide - Bihar Teacher Portal

## ✅ Both Applications Are Running!

### Frontend
- **URL**: http://localhost:4200/
- **Status**: ✅ Running

### Backend
- **URL**: http://localhost:5162
- **Status**: ✅ Running

---

## 🎯 Quick Test Steps

### 1. Test Admin Login (2 minutes)
```
1. Go to: http://localhost:4200/login
2. Username: admin
3. Password: admin
4. Click "Sign In"
5. You'll be redirected to Teacher Management
```

### 2. Test Teacher Registration (3 minutes)
```
1. Go to: http://localhost:4200/register
2. Fill in:
   - Username: teacher1
   - Email: teacher1@test.com
   - Role: Teacher
   - Password: test123
   - Confirm Password: test123
3. Click "Create Account"
4. See message: "Your account is pending admin approval"
```

### 3. Test Admin Approval (2 minutes)
```
1. Login as admin (if not already)
2. Click "User Onboarding" in navigation
3. See teacher1 in pending users
4. Click "Approve" button
5. User is now approved!
```

### 4. Test Teacher Login & Profile (5 minutes)
```
1. Logout (if logged in as admin)
2. Login as teacher1 (password: test123)
3. You'll be redirected to Self-Declaration Form
4. Fill in the form:
   - Teacher Name: John Doe
   - Address: 123 Main St
   - District: Patna
   - Pincode: (select from dropdown)
   - School: (select any school)
   - Class Teaching: Class 10
   - Subject: Mathematics
   - Qualification: B.Ed, M.Sc
   - Contact Number: 9876543210
   - Date of Joining: (select date)
5. Click "Save Profile"
6. You'll be redirected to User Dashboard
```

### 5. Test Document Upload (3 minutes)
```
1. From User Dashboard, click "My Documents"
2. Select Document Type: Resume
3. Choose a file (PDF, DOC, or image)
4. Add remarks: "My teaching resume"
5. Click "Upload Document"
6. See success message
7. Document appears in card view
8. Try: View, Download, Email, Delete
```

---

## 📋 All Available Routes

### Public Routes
- `/register` - Registration page
- `/login` - Login page
- `/notices` - Notice board
- `/about` - About us page
- `/search` - Search page

### Teacher Routes (Requires Login)
- `/user-dashboard` - Teacher dashboard
- `/self-declaration` - Complete profile
- `/my-documents` - Upload/manage documents

### Admin Routes (Requires Admin Login)
- `/user-onboarding` - Approve/reject users
- `/teachers` - Teacher management
- `/schools` - School management
- `/teacher-report` - Teacher reports
- `/teacher-documents/:id` - Teacher documents

---

## 🔑 Default Credentials

### Admin Account
```
Username: admin
Password: admin
```

### Test Teacher Account (After Registration & Approval)
```
Username: teacher1
Password: test123
```

---

## 🎨 Key Features to Test

### ✅ Registration
- [x] Role selection (Teacher/Admin)
- [x] Separate username and email
- [x] Password confirmation
- [x] Approval pending message

### ✅ Login
- [x] Role-based redirect
- [x] Approval check
- [x] Profile check for teachers

### ✅ Self-Declaration
- [x] Pre-filled email
- [x] District-Pincode integration
- [x] School selection
- [x] Form validation

### ✅ My Documents
- [x] Upload documents
- [x] View documents (card view)
- [x] Download documents
- [x] Email documents
- [x] Delete documents

### ✅ User Dashboard
- [x] Profile summary
- [x] Document count
- [x] Quick actions
- [x] Recent documents

### ✅ User Onboarding (Admin)
- [x] Pending users list
- [x] Approve users
- [x] Reject users with reason
- [x] All users view

---

## 🐛 Troubleshooting

### Frontend Not Loading?
```bash
# Check if process is running
# If not, restart:
cd Frontend
npm start
```

### Backend Not Responding?
```bash
# Check if process is running
# If not, restart:
cd Backend/WebAPI
dotnet run
```

### Database Issues?
```bash
# Apply migrations:
cd Backend
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

### Clear Browser Cache
```
Press Ctrl+Shift+R (Windows/Linux)
Press Cmd+Shift+R (Mac)
```

---

## 📊 Database Info

### Location
`Backend/WebAPI/teacherportal.db`

### Tables
- Users (with Role, IsApproved, IsActive)
- UserProfiles (teacher self-declarations)
- TeacherDocuments (with UserId support)
- Schools (10 pre-loaded Bihar schools)
- Teachers
- Notices
- NoticeReplies

### Pre-loaded Data
- 1 Admin user (admin/admin)
- 10 Bihar government schools
- 38 Bihar districts with 758+ pincodes

---

## 🎯 Success Indicators

You'll know everything is working when:

1. ✅ You can register as a teacher
2. ✅ Admin can see pending user
3. ✅ Admin can approve user
4. ✅ Teacher can login after approval
5. ✅ Teacher sees self-declaration form
6. ✅ Teacher can complete profile
7. ✅ Teacher can upload documents
8. ✅ Teacher can view dashboard
9. ✅ Admin can access all features

---

## 🎉 You're All Set!

The Bihar Teacher Portal is fully functional with:
- ✅ Role-based authentication
- ✅ Admin approval workflow
- ✅ Teacher self-declaration
- ✅ Document management
- ✅ User dashboard
- ✅ Fluent UI design

**Enjoy testing!** 🚀
