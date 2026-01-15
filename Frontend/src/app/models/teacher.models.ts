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
  isActive: boolean;
  documentCount?: number;
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
export interface TeacherReport {
  id: number;
  teacherName: string;
  schoolName: string;
  district: string;
  pincode: string;
  contactNumber: string;
  email: string;
  address: string;
  classTeaching: string;
  subject: string;
  dateOfJoining: string;
  isActive: boolean;
}

export interface TeacherReportSearchRequest {
  searchTerm?: string;
  teacherName?: string;
  schoolName?: string;
  district?: string;
  pincode?: string;
  contactNumber?: string;
  page: number;
  pageSize: number;
  sortBy?: string;
  sortDirection?: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}