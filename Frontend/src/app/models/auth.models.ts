export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
  role: string;
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  userName: string;
  email: string;
  role: string;
  isApproved: boolean;
  isActive: boolean;
}

export interface User {
  id: number;
  userName: string;
  email: string;
  role: string;
  isApproved: boolean;
  isActive: boolean;
  approvedByUserId?: number;
  approvedDate?: string;
  rejectionReason?: string;
  createdDate: string;
}

export interface PendingUsersResponse {
  pendingUsers: User[];
  totalCount: number;
}

export interface SelfDeclaration {
  id?: number;
  userId: number;
  teacherName: string;
  address: string;
  district: string;
  pincode: string;
  schoolId: number;
  schoolName?: string;
  classTeaching: string;
  subject: string;
  qualification: string;
  contactNumber: string;
  email: string;
  dateOfJoining: string;
  isActive: boolean;
  createdDate?: string;
  updatedDate?: string;
}
