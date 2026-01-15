# Self-Declaration Form Implementation Plan

## Overview
After successful login, users will see a self-declaration form (similar to Add Teacher form) where they can:
1. Fill in their personal and professional details
2. Upload documents
3. View only their own uploaded documents

## Architecture

### User Flow
```
Login Success
    ↓
Check if user has self-declaration
    ↓
NO → Redirect to Self-Declaration Form
YES → Redirect to Dashboard (with view/edit option)
```

### Backend Requirements

#### New Entity: UserProfile (Self-Declaration)
```csharp
public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; } // FK to User
    public string TeacherName { get; set; }
    public string Address { get; set; }
    public string District { get; set; }
    public string Pincode { get; set; }
    public int SchoolId { get; set; }
    public string ClassTeaching { get; set; }
    public string Subject { get; set; }
    public string Qualification { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public DateTime DateOfJoining { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    // Navigation
    public User User { get; set; }
    public School School { get; set; }
}
```

#### API Endpoints Needed
```
POST   /api/userprofile          - Create self-declaration
GET    /api/userprofile/my        - Get current user's profile
PUT    /api/userprofile/{id}      - Update self-declaration
GET    /api/userprofile/{id}      - Get specific profile (admin only)

// Documents - Filter by current user
GET    /api/teacherdocument/my-documents  - Get current user's documents only
```

### Frontend Components

#### 1. Self-Declaration Component
**Path**: `Frontend/src/app/components/self-declaration/`

**Features**:
- Form similar to Add Teacher
- Pre-fill email from logged-in user
- District-Pincode integration
- School selection
- Save/Update functionality
- Navigation to document upload

#### 2. My Documents Component  
**Path**: `Frontend/src/app/components/my-documents/`

**Features**:
- Upload documents (same as teacher-documents)
- View only user's own documents
- Download documents
- Delete documents
- Send via email

#### 3. User Dashboard Component
**Path**: `Frontend/src/app/components/user-dashboard/`

**Features**:
- Welcome message
- Profile summary
- Document count
- Quick actions (Edit Profile, Upload Documents)
- View documents

#### 4. Updated Register Component
**Features**:
- Role selection dropdown (Admin/Teacher)
- Success message with approval notice

#### 5. Updated Login Component
**Features**:
- Handle role-based redirection
- Admin → Admin Dashboard (Teacher Management)
- Teacher → Check if profile exists
  - No Profile → Self-Declaration Form
  - Has Profile → User Dashboard

#### 6. User Onboarding Component (Admin Only)
**Path**: `Frontend/src/app/components/user-onboarding/`

**Features**:
- List pending users
- Approve/Reject buttons
- View user details
- Filter and search

### Routes Configuration

```typescript
{
  path: 'register', component: RegisterComponent
},
{
  path: 'login', component: LoginComponent
},
{
  path: 'self-declaration', component: SelfDeclarationComponent,
  canActivate: [AuthGuard]
},
{
  path: 'my-documents', component: MyDocumentsComponent,
  canActivate: [AuthGuard]
},
{
  path: 'user-dashboard', component: UserDashboardComponent,
  canActivate: [AuthGuard]
},
{
  path: 'user-onboarding', component: UserOnboardingComponent,
  canActivate: [AuthGuard, AdminGuard]
},
{
  path: 'teachers', component: TeacherManagementComponent,
  canActivate: [AuthGuard, AdminGuard]
}
```

### Auth Guards

#### AuthGuard
- Check if user is authenticated
- Redirect to login if not

#### AdminGuard
- Check if user role is Admin
- Redirect to user-dashboard if not admin

### Services

#### UserProfileService
```typescript
export class UserProfileService {
  getMyProfile(): Observable<UserProfile>
  createProfile(profile: UserProfile): Observable<UserProfile>
  updateProfile(id: number, profile: UserProfile): Observable<UserProfile>
  hasProfile(): Observable<boolean>
}
```

#### Updated DocumentService
```typescript
export class DocumentService {
  // Existing methods...
  
  getMyDocuments(): Observable<TeacherDocument[]>
  // Filter documents by current user
}
```

## Implementation Steps

### Phase 1: Backend (Priority)
1. ✅ Create UserProfile entity
2. ✅ Create UserProfile repository
3. ✅ Create UserProfile service
4. ✅ Create UserProfile controller
5. ✅ Update TeacherDocument to link with UserId
6. ✅ Add endpoint to get user's own documents
7. ✅ Create migration
8. ✅ Apply migration

### Phase 2: Frontend Core
1. ✅ Update auth.models.ts
2. ✅ Update auth.service.ts (role management)
3. ✅ Create user-profile.service.ts
4. ✅ Update document.service.ts (my documents)
5. ✅ Create auth.guard.ts
6. ✅ Create admin.guard.ts

### Phase 3: Frontend Components
1. ✅ Update Register Component (add role selection)
2. ✅ Update Login Component (role-based redirect)
3. ✅ Create Self-Declaration Component
4. ✅ Create My Documents Component
5. ✅ Create User Dashboard Component
6. ✅ Create User Onboarding Component (Admin)

### Phase 4: Integration & Testing
1. Test registration with role
2. Test login with approval check
3. Test self-declaration form
4. Test document upload (user-specific)
5. Test admin approval workflow
6. Test role-based navigation

## Data Flow Examples

### New User Registration Flow
```
1. User registers with role "Teacher"
2. Account created (IsApproved = false)
3. User tries to login → "Pending approval" message
4. Admin logs in → sees user in onboarding list
5. Admin approves user
6. User logs in successfully
7. No profile exists → Redirect to Self-Declaration
8. User fills form and saves
9. Redirect to User Dashboard
10. User can upload documents
```

### Existing User Login Flow
```
1. User logs in
2. Check role:
   - Admin → Teacher Management
   - Teacher → Check profile
     - Has profile → User Dashboard
     - No profile → Self-Declaration Form
```

### Document Upload Flow
```
1. User on Dashboard clicks "Upload Documents"
2. Redirect to My Documents page
3. User uploads document
4. Document saved with UserId
5. Only user can see their documents
6. Admin can see all documents in Teacher Management
```

## Security Considerations

1. **User Isolation**
   - Users can only see/edit their own profile
   - Users can only see/manage their own documents
   - Admin can see all profiles and documents

2. **Authorization**
   - All endpoints require authentication
   - Admin endpoints require Admin role
   - User-specific endpoints check ownership

3. **Data Validation**
   - Validate user owns the resource before update/delete
   - Validate role before allowing access
   - Validate approval status before allowing login

## UI/UX Considerations

1. **First-Time User Experience**
   - Clear message: "Complete your profile to continue"
   - Progress indicator
   - Help text for each field

2. **Dashboard**
   - Welcome message with user name
   - Profile completion status
   - Document count
   - Quick actions

3. **Admin Experience**
   - Clear pending user count
   - Easy approve/reject actions
   - User details preview
   - Bulk actions (future)

## Database Schema

```sql
-- New Table
UserProfiles
- Id (PK)
- UserId (FK to Users, Unique)
- TeacherName
- Address
- District
- Pincode
- SchoolId (FK to Schools)
- ClassTeaching
- Subject
- Qualification
- ContactNumber
- Email
- DateOfJoining
- IsActive
- CreatedDate
- UpdatedDate

-- Updated Table
TeacherDocuments
- Add: UserId (FK to Users)
- Keep: TeacherId (FK to Teachers) - for admin-created teachers
- Logic: If UserId is set, it's user-uploaded; if TeacherId is set, it's admin-uploaded
```

## Testing Checklist

### Backend
- [ ] Create user profile
- [ ] Get user profile
- [ ] Update user profile
- [ ] Get user's documents only
- [ ] Upload document with user ID
- [ ] Admin can see all documents
- [ ] User cannot see other user's documents

### Frontend
- [ ] Register with role selection
- [ ] Login redirects based on role
- [ ] Self-declaration form works
- [ ] Document upload works
- [ ] User sees only their documents
- [ ] Admin sees pending users
- [ ] Admin can approve/reject
- [ ] Navigation works correctly

## Future Enhancements

1. **Profile Verification**
   - Admin can verify profile information
   - Verification badge

2. **Document Verification**
   - Admin can verify uploaded documents
   - Verification status

3. **Notifications**
   - Email notification on approval
   - Email notification on rejection
   - Reminder to complete profile

4. **Analytics**
   - Profile completion rate
   - Document upload statistics
   - Approval time metrics

---

**Status**: Planning Complete ✅  
**Next**: Backend Implementation
