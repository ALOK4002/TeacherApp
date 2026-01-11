export interface Teacher {
  id: number;
  teacherName: string;
  address: string;
  district: string;
  pincode: string;
  schoolId: number;
  schoolName: string;
  classTeaching: string;
  subject: string;
  qualification: string;
  contactNumber: string;
  email: string;
  dateOfJoining: string;
  salary: number;
  isActive: boolean;
}

export interface CreateTeacher {
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
  dateOfJoining: string;
  salary: number;
}

export interface UpdateTeacher {
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
  dateOfJoining: string;
  salary: number;
  isActive: boolean;
}

export interface District {
  name: string;
  pincodes: string[];
}

export interface Pincode {
  pincode: string;
  district: string;
}