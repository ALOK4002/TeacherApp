import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TeacherService } from '../../services/teacher.service';
import { SchoolService } from '../../services/school.service';
import { AuthService } from '../../services/auth.service';
import { Teacher, CreateTeacher, District } from '../../models/teacher.models';
import { School } from '../../models/school.models';
import { NoticeWidgetComponent } from '../notice-widget/notice-widget.component';

@Component({
  selector: 'app-teacher-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NoticeWidgetComponent],
  template: `
    <div class="teacher-management-container">
      <div class="header">
        <h1>Teacher Management System</h1>
        <div class="header-actions">
          <span class="welcome-text">Welcome, {{ userName }}!</span>
          <button (click)="showAddModal = true" class="btn-add">Add Teacher</button>
          <button (click)="goToNoticeBoard()" class="btn-secondary">Notice Board</button>
          <button (click)="goToAbout()" class="btn-secondary">About Us</button>
          <button (click)="goToSchools()" class="btn-secondary">Manage Schools</button>
          <button (click)="logout()" class="btn-logout">Logout</button>
        </div>
      </div>

      <div class="filters">
        <div class="filter-group">
          <label for="districtFilter">Filter by District:</label>
          <select id="districtFilter" [(ngModel)]="selectedDistrict" (change)="filterByDistrict()" class="form-control">
            <option value="">All Districts</option>
            <option *ngFor="let district of districts" [value]="district.name">{{ district.name }}</option>
          </select>
        </div>
        <div class="filter-group">
          <label for="searchFilter">Search Teachers:</label>
          <input type="text" id="searchFilter" [(ngModel)]="searchTerm" (input)="filterTeachers()" 
                 placeholder="Search by name, email, or school..." class="form-control">
        </div>
      </div>

      <div class="table-container">
        <div class="loading" *ngIf="isLoading">Loading teachers...</div>
        <div class="error-message" *ngIf="errorMessage">{{ errorMessage }}</div>
        
        <table class="teachers-table" *ngIf="!isLoading && !errorMessage">
          <thead>
            <tr>
              <th>Name</th>
              <th>District</th>
              <th>Pincode</th>
              <th>School</th>
              <th>Class</th>
              <th>Subject</th>
              <th>Qualification</th>
              <th>Contact</th>
              <th>Email</th>
              <th>Salary</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let teacher of filteredTeachers" [class.inactive]="!teacher.isActive">
              <td>{{ teacher.teacherName }}</td>
              <td>{{ teacher.district }}</td>
              <td>{{ teacher.pincode }}</td>
              <td>{{ teacher.schoolName }}</td>
              <td>{{ teacher.classTeaching }}</td>
              <td>{{ teacher.subject }}</td>
              <td>{{ teacher.qualification }}</td>
              <td>{{ teacher.contactNumber }}</td>
              <td>{{ teacher.email }}</td>
              <td>₹{{ teacher.salary | number:'1.0-0' }}</td>
              <td>
                <span class="status" [class.active]="teacher.isActive" [class.inactive]="!teacher.isActive">
                  {{ teacher.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td>
                <button (click)="editTeacher(teacher)" class="btn-edit">Edit</button>
                <button (click)="toggleTeacherStatus(teacher)" class="btn-toggle" 
                        [class.activate]="!teacher.isActive" [class.deactivate]="teacher.isActive">
                  {{ teacher.isActive ? 'Deactivate' : 'Activate' }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Add/Edit Modal -->
      <div class="modal-overlay" *ngIf="showAddModal || showEditModal" (click)="closeModal()">
        <div class="modal-content" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h2>{{ showAddModal ? 'Add New Teacher' : 'Edit Teacher' }}</h2>
            <button class="close-btn" (click)="closeModal()">&times;</button>
          </div>
          <form [formGroup]="teacherForm" (ngSubmit)="saveTeacher()" class="teacher-form">
            <div class="form-row">
              <div class="form-group">
                <label for="teacherName">Teacher Name *</label>
                <input type="text" id="teacherName" formControlName="teacherName" class="form-control">
                <div class="error-text" *ngIf="teacherForm.get('teacherName')?.invalid && teacherForm.get('teacherName')?.touched">
                  Teacher name is required
                </div>
              </div>
              <div class="form-group">
                <label for="email">Email *</label>
                <input type="email" id="email" formControlName="email" class="form-control">
                <div class="error-text" *ngIf="teacherForm.get('email')?.invalid && teacherForm.get('email')?.touched">
                  Valid email is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="contactNumber">Contact Number *</label>
                <input type="tel" id="contactNumber" formControlName="contactNumber" class="form-control" 
                       placeholder="10-digit mobile number">
                <div class="error-text" *ngIf="teacherForm.get('contactNumber')?.invalid && teacherForm.get('contactNumber')?.touched">
                  10-digit contact number is required
                </div>
              </div>
              <div class="form-group">
                <label for="qualification">Qualification *</label>
                <input type="text" id="qualification" formControlName="qualification" class="form-control" 
                       placeholder="e.g., B.Ed, M.A, B.Sc">
                <div class="error-text" *ngIf="teacherForm.get('qualification')?.invalid && teacherForm.get('qualification')?.touched">
                  Qualification is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group full-width">
                <label for="address">Address *</label>
                <textarea id="address" formControlName="address" class="form-control" rows="2" 
                          placeholder="Complete address"></textarea>
                <div class="error-text" *ngIf="teacherForm.get('address')?.invalid && teacherForm.get('address')?.touched">
                  Address is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="district">District *</label>
                <select id="district" formControlName="district" (change)="onDistrictChange()" class="form-control">
                  <option value="">Select District</option>
                  <option *ngFor="let district of districts" [value]="district.name">{{ district.name }}</option>
                </select>
                <div class="error-text" *ngIf="teacherForm.get('district')?.invalid && teacherForm.get('district')?.touched">
                  District selection is required
                </div>
              </div>
              <div class="form-group">
                <label for="pincode">Pincode *</label>
                <select id="pincode" formControlName="pincode" (change)="onPincodeChange()" class="form-control">
                  <option value="">Select Pincode</option>
                  <option *ngFor="let pincode of availablePincodes" [value]="pincode">{{ pincode }}</option>
                </select>
                <div class="error-text" *ngIf="teacherForm.get('pincode')?.invalid && teacherForm.get('pincode')?.touched">
                  Pincode selection is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="schoolId">School *</label>
                <select id="schoolId" formControlName="schoolId" class="form-control">
                  <option value="">Select School</option>
                  <option *ngFor="let school of filteredSchools" [value]="school.id">
                    {{ school.schoolName }} ({{ school.district }})
                  </option>
                </select>
                <div class="error-text" *ngIf="teacherForm.get('schoolId')?.invalid && teacherForm.get('schoolId')?.touched">
                  School selection is required
                </div>
              </div>
              <div class="form-group">
                <label for="classTeaching">Class Teaching *</label>
                <select id="classTeaching" formControlName="classTeaching" class="form-control">
                  <option value="">Select Class</option>
                  <option value="Nursery">Nursery</option>
                  <option value="LKG">LKG</option>
                  <option value="UKG">UKG</option>
                  <option value="Class 1">Class 1</option>
                  <option value="Class 2">Class 2</option>
                  <option value="Class 3">Class 3</option>
                  <option value="Class 4">Class 4</option>
                  <option value="Class 5">Class 5</option>
                  <option value="Class 6">Class 6</option>
                  <option value="Class 7">Class 7</option>
                  <option value="Class 8">Class 8</option>
                  <option value="Class 9">Class 9</option>
                  <option value="Class 10">Class 10</option>
                  <option value="Class 11">Class 11</option>
                  <option value="Class 12">Class 12</option>
                  <option value="Multiple Classes">Multiple Classes</option>
                </select>
                <div class="error-text" *ngIf="teacherForm.get('classTeaching')?.invalid && teacherForm.get('classTeaching')?.touched">
                  Class selection is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="subject">Subject *</label>
                <input type="text" id="subject" formControlName="subject" class="form-control" 
                       placeholder="e.g., Mathematics, English, Science">
                <div class="error-text" *ngIf="teacherForm.get('subject')?.invalid && teacherForm.get('subject')?.touched">
                  Subject is required
                </div>
              </div>
              <div class="form-group">
                <label for="salary">Monthly Salary (₹) *</label>
                <input type="number" id="salary" formControlName="salary" class="form-control" min="0" step="100">
                <div class="error-text" *ngIf="teacherForm.get('salary')?.invalid && teacherForm.get('salary')?.touched">
                  Valid salary is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="dateOfJoining">Date of Joining *</label>
                <input type="date" id="dateOfJoining" formControlName="dateOfJoining" class="form-control">
                <div class="error-text" *ngIf="teacherForm.get('dateOfJoining')?.invalid && teacherForm.get('dateOfJoining')?.touched">
                  Date of joining is required
                </div>
              </div>
              <div class="form-group" *ngIf="showEditModal">
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="isActive"> Active
                </label>
              </div>
            </div>

            <div class="form-actions">
              <button type="button" (click)="closeModal()" class="btn-cancel">Cancel</button>
              <button type="submit" [disabled]="teacherForm.invalid || isSaving" class="btn-save">
                {{ isSaving ? 'Saving...' : (showAddModal ? 'Add Teacher' : 'Save Changes') }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- Notice Board Widget -->
      <app-notice-widget></app-notice-widget>
    </div>
  `,
  styles: [`
    .teacher-management-container {
      padding: 20px;
      max-width: 1400px;
      margin: 0 auto;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      padding-bottom: 20px;
      border-bottom: 2px solid #e0e0e0;
      flex-wrap: wrap;
      gap: 15px;
    }

    .header h1 {
      color: #2c3e50;
      margin: 0;
      font-size: 2rem;
    }

    .header-actions {
      display: flex;
      align-items: center;
      gap: 15px;
      flex-wrap: wrap;
    }

    .welcome-text {
      color: #34495e;
      font-weight: 500;
    }

    .btn-add, .btn-secondary, .btn-logout {
      padding: 8px 16px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      text-decoration: none;
      display: inline-block;
    }

    .btn-add {
      background-color: #28a745;
      color: white;
    }

    .btn-add:hover {
      background-color: #218838;
    }

    .btn-secondary {
      background-color: #007bff;
      color: white;
    }

    .btn-secondary:hover {
      background-color: #0056b3;
    }

    .btn-logout {
      background-color: #e74c3c;
      color: white;
    }

    .btn-logout:hover {
      background-color: #c0392b;
    }

    .filters {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
      padding: 20px;
      background-color: #f8f9fa;
      border-radius: 8px;
      flex-wrap: wrap;
    }

    .filter-group {
      display: flex;
      flex-direction: column;
      gap: 5px;
      min-width: 200px;
    }

    .filter-group label {
      font-weight: 500;
      color: #495057;
    }

    .form-control {
      padding: 8px 12px;
      border: 1px solid #ced4da;
      border-radius: 4px;
      font-size: 14px;
    }

    .form-control:focus {
      outline: none;
      border-color: #007bff;
      box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
    }

    .table-container {
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }

    .loading {
      text-align: center;
      padding: 40px;
      color: #6c757d;
    }

    .error-message {
      text-align: center;
      padding: 20px;
      color: #dc3545;
      background-color: #f8d7da;
      border: 1px solid #f5c6cb;
      border-radius: 4px;
      margin: 20px;
    }

    .teachers-table {
      width: 100%;
      border-collapse: collapse;
      font-size: 13px;
    }

    .teachers-table th {
      background-color: #343a40;
      color: white;
      padding: 12px 8px;
      text-align: left;
      font-weight: 600;
      position: sticky;
      top: 0;
    }

    .teachers-table td {
      padding: 10px 8px;
      border-bottom: 1px solid #dee2e6;
      vertical-align: middle;
    }

    .teachers-table tr:hover {
      background-color: #f8f9fa;
    }

    .teachers-table tr.inactive {
      opacity: 0.6;
      background-color: #f8f9fa;
    }

    .status {
      padding: 4px 8px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
    }

    .status.active {
      background-color: #d4edda;
      color: #155724;
    }

    .status.inactive {
      background-color: #f8d7da;
      color: #721c24;
    }

    .btn-edit, .btn-toggle {
      padding: 6px 12px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 12px;
      margin-right: 5px;
    }

    .btn-edit {
      background-color: #007bff;
      color: white;
    }

    .btn-edit:hover {
      background-color: #0056b3;
    }

    .btn-toggle.activate {
      background-color: #28a745;
      color: white;
    }

    .btn-toggle.deactivate {
      background-color: #ffc107;
      color: #212529;
    }

    .btn-toggle:hover {
      opacity: 0.8;
    }

    /* Modal Styles */
    .modal-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.5);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }

    .modal-content {
      background: white;
      border-radius: 8px;
      width: 90%;
      max-width: 900px;
      max-height: 90vh;
      overflow-y: auto;
    }

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20px;
      border-bottom: 1px solid #dee2e6;
    }

    .modal-header h2 {
      margin: 0;
      color: #2c3e50;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 24px;
      cursor: pointer;
      color: #6c757d;
    }

    .close-btn:hover {
      color: #495057;
    }

    .teacher-form {
      padding: 20px;
    }

    .form-row {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
    }

    .form-group {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .form-group.full-width {
      flex: 2;
    }

    .form-group label {
      font-weight: 500;
      color: #495057;
    }

    .checkbox-label {
      flex-direction: row !important;
      align-items: center;
      gap: 8px !important;
    }

    .error-text {
      color: #dc3545;
      font-size: 12px;
    }

    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 30px;
      padding-top: 20px;
      border-top: 1px solid #dee2e6;
    }

    .btn-cancel, .btn-save {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
    }

    .btn-cancel {
      background-color: #6c757d;
      color: white;
    }

    .btn-cancel:hover {
      background-color: #545b62;
    }

    .btn-save {
      background-color: #28a745;
      color: white;
    }

    .btn-save:hover:not(:disabled) {
      background-color: #218838;
    }

    .btn-save:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    @media (max-width: 768px) {
      .form-row {
        flex-direction: column;
      }
      
      .filters {
        flex-direction: column;
      }
      
      .teachers-table {
        font-size: 11px;
      }
      
      .teachers-table th,
      .teachers-table td {
        padding: 6px 4px;
      }

      .header {
        flex-direction: column;
        align-items: flex-start;
      }

      .header-actions {
        width: 100%;
        justify-content: flex-start;
      }
    }
  `]
})
export class TeacherManagementComponent implements OnInit {
  teachers: Teacher[] = [];
  filteredTeachers: Teacher[] = [];
  schools: School[] = [];
  filteredSchools: School[] = [];
  districts: District[] = [];
  availablePincodes: string[] = [];
  selectedDistrict: string = '';
  searchTerm: string = '';
  isLoading = false;
  errorMessage = '';
  userName = '';

  // Modal states
  showAddModal = false;
  showEditModal = false;
  teacherForm: FormGroup;
  currentTeacher: Teacher | null = null;
  isSaving = false;

  constructor(
    private teacherService: TeacherService,
    private schoolService: SchoolService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.teacherForm = this.fb.group({
      teacherName: ['', Validators.required],
      address: ['', Validators.required],
      district: ['', Validators.required],
      pincode: ['', Validators.required],
      schoolId: ['', [Validators.required, Validators.min(1)]],
      classTeaching: ['', Validators.required],
      subject: ['', Validators.required],
      qualification: ['', Validators.required],
      contactNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      email: ['', [Validators.required, Validators.email]],
      dateOfJoining: ['', Validators.required],
      salary: ['', [Validators.required, Validators.min(1)]],
      isActive: [true]
    });
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
    this.loadInitialData();
  }

  loadInitialData() {
    this.loadTeachers();
    this.loadSchools();
    this.loadDistricts();
  }

  loadTeachers() {
    this.isLoading = true;
    this.errorMessage = '';

    this.teacherService.getAllTeachers().subscribe({
      next: (teachers) => {
        this.teachers = teachers;
        this.filteredTeachers = teachers;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load teachers. Please try again.';
        this.isLoading = false;
        console.error('Error loading teachers:', error);
      }
    });
  }

  loadSchools() {
    this.schoolService.getAllSchools().subscribe({
      next: (schools) => {
        this.schools = schools;
        this.filteredSchools = schools;
      },
      error: (error) => {
        console.error('Error loading schools:', error);
      }
    });
  }

  loadDistricts() {
    this.teacherService.getBiharDistricts().subscribe({
      next: (districts) => {
        this.districts = districts;
      },
      error: (error) => {
        console.error('Error loading districts:', error);
      }
    });
  }

  onDistrictChange() {
    const selectedDistrict = this.teacherForm.get('district')?.value;
    if (selectedDistrict) {
      const district = this.districts.find(d => d.name === selectedDistrict);
      this.availablePincodes = district?.pincodes || [];
      
      // Filter schools by district
      this.filteredSchools = this.schools.filter(s => s.district === selectedDistrict);
      
      // Reset pincode and school selection
      this.teacherForm.patchValue({ pincode: '', schoolId: '' });
    } else {
      this.availablePincodes = [];
      this.filteredSchools = this.schools;
    }
  }

  onPincodeChange() {
    const selectedPincode = this.teacherForm.get('pincode')?.value;
    if (selectedPincode) {
      // Auto-select district based on pincode
      this.teacherService.getDistrictByPincode(selectedPincode).subscribe({
        next: (response) => {
          if (response.district && this.teacherForm.get('district')?.value !== response.district) {
            this.teacherForm.patchValue({ district: response.district });
            this.onDistrictChange();
          }
        },
        error: (error) => {
          console.error('Error getting district by pincode:', error);
        }
      });
    }
  }

  filterByDistrict() {
    this.applyFilters();
  }

  filterTeachers() {
    this.applyFilters();
  }

  applyFilters() {
    let filtered = this.teachers;

    if (this.selectedDistrict) {
      filtered = filtered.filter(teacher => teacher.district === this.selectedDistrict);
    }

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(teacher => 
        teacher.teacherName.toLowerCase().includes(term) ||
        teacher.email.toLowerCase().includes(term) ||
        teacher.schoolName.toLowerCase().includes(term) ||
        teacher.subject.toLowerCase().includes(term)
      );
    }

    this.filteredTeachers = filtered;
  }

  editTeacher(teacher: Teacher) {
    this.currentTeacher = teacher;
    this.teacherForm.patchValue({
      teacherName: teacher.teacherName,
      address: teacher.address,
      district: teacher.district,
      pincode: teacher.pincode,
      schoolId: teacher.schoolId,
      classTeaching: teacher.classTeaching,
      subject: teacher.subject,
      qualification: teacher.qualification,
      contactNumber: teacher.contactNumber,
      email: teacher.email,
      dateOfJoining: teacher.dateOfJoining.split('T')[0],
      salary: teacher.salary,
      isActive: teacher.isActive
    });
    
    // Load pincodes for the selected district
    this.onDistrictChange();
    
    this.showEditModal = true;
  }

  saveTeacher() {
    if (this.teacherForm.valid) {
      this.isSaving = true;
      
      if (this.showAddModal) {
        const createData: CreateTeacher = this.teacherForm.value;
        this.teacherService.createTeacher(createData).subscribe({
          next: (newTeacher) => {
            this.teachers.push(newTeacher);
            this.applyFilters();
            this.closeModal();
            this.isSaving = false;
          },
          error: (error) => {
            console.error('Error creating teacher:', error);
            this.errorMessage = 'Failed to create teacher. Please try again.';
            this.isSaving = false;
          }
        });
      } else if (this.showEditModal && this.currentTeacher) {
        const updateData = {
          id: this.currentTeacher.id,
          ...this.teacherForm.value
        };

        this.teacherService.updateTeacher(this.currentTeacher.id, updateData).subscribe({
          next: (updatedTeacher) => {
            const index = this.teachers.findIndex(t => t.id === updatedTeacher.id);
            if (index !== -1) {
              this.teachers[index] = updatedTeacher;
              this.applyFilters();
            }
            this.closeModal();
            this.isSaving = false;
          },
          error: (error) => {
            console.error('Error updating teacher:', error);
            this.errorMessage = 'Failed to update teacher. Please try again.';
            this.isSaving = false;
          }
        });
      }
    }
  }

  toggleTeacherStatus(teacher: Teacher) {
    const updateData = {
      ...teacher,
      isActive: !teacher.isActive,
      dateOfJoining: teacher.dateOfJoining.split('T')[0]
    };

    this.teacherService.updateTeacher(teacher.id, updateData).subscribe({
      next: (updatedTeacher) => {
        const index = this.teachers.findIndex(t => t.id === updatedTeacher.id);
        if (index !== -1) {
          this.teachers[index] = updatedTeacher;
          this.applyFilters();
        }
      },
      error: (error) => {
        console.error('Error toggling teacher status:', error);
        this.errorMessage = 'Failed to update teacher status. Please try again.';
      }
    });
  }

  closeModal() {
    this.showAddModal = false;
    this.showEditModal = false;
    this.currentTeacher = null;
    this.teacherForm.reset();
    this.teacherForm.patchValue({ isActive: true });
    this.availablePincodes = [];
    this.filteredSchools = this.schools;
  }

  goToSchools() {
    this.router.navigate(['/schools']);
  }

  goToNoticeBoard() {
    this.router.navigate(['/notices']);
  }

  goToAbout() {
    this.router.navigate(['/about']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}