export interface School {
  id: number;
  schoolName: string;
  schoolCode: string;
  district: string;
  block: string;
  village: string;
  schoolType: string;
  managementType: string;
  totalStudents: number;
  totalTeachers: number;
  principalName: string;
  contactNumber: string;
  email: string;
  isActive: boolean;
  establishedDate: string;
}

export interface CreateSchool {
  schoolName: string;
  schoolCode: string;
  district: string;
  block: string;
  village: string;
  schoolType: string;
  managementType: string;
  totalStudents: number;
  totalTeachers: number;
  principalName: string;
  contactNumber: string;
  email: string;
  establishedDate: string;
}

export interface UpdateSchool {
  id: number;
  schoolName: string;
  schoolCode: string;
  district: string;
  block: string;
  village: string;
  schoolType: string;
  managementType: string;
  totalStudents: number;
  totalTeachers: number;
  principalName: string;
  contactNumber: string;
  email: string;
  isActive: boolean;
  establishedDate: string;
}