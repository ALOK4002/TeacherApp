export interface UserProfile {
  id: number;
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
  dateOfJoining: Date;
  isActive: boolean;
  createdDate: Date;
  updatedDate: Date;
}

export interface CreateUserProfile {
  teacherName: string;
  address: string;
  district: string;
  pincode: string;
  schoolId: number;
  classTeaching: string;
  subject: string;
  qualification: string;
  contactNumber: string;
  email: string;
  dateOfJoining: Date;
}

export interface UpdateUserProfile {
  id: number;
  teacherName: string;
  address: string;
  district: string;
  pincode: string;
  schoolId: number;
  classTeaching: string;
  subject: string;
  qualification: string;
  contactNumber: string;
  email: string;
  dateOfJoining: Date;
}
