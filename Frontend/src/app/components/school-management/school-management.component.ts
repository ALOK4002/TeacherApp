import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SchoolService } from '../../services/school.service';
import { AuthService } from '../../services/auth.service';
import { School, UpdateSchool } from '../../models/school.models';

@Component({
  selector: 'app-school-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="school-management-container">
      <div class="header">
        <h1>Bihar Government Schools Management</h1>
        <div class="header-actions">
          <span class="welcome-text">Welcome, {{ userName }}!</span>
          <button (click)="goToNoticeBoard()" class="btn-secondary">Notice Board</button>
          <button (click)="goToAbout()" class="btn-secondary">About Us</button>
          <button (click)="goToTeachers()" class="btn-secondary">Manage Teachers</button>
          <button (click)="logout()" class="btn-logout">Logout</button>
        </div>
      </div>

      <div class="filters">
        <div class="filter-group">
          <label for="districtFilter">Filter by District:</label>
          <select id="districtFilter" [(ngModel)]="selectedDistrict" (change)="filterByDistrict()" class="form-control">
            <option value="">All Districts</option>
            <option *ngFor="let district of districts" [value]="district">{{ district }}</option>
          </select>
        </div>
        <div class="filter-group">
          <label for="searchFilter">Search Schools:</label>
          <input type="text" id="searchFilter" [(ngModel)]="searchTerm" (input)="filterSchools()" 
                 placeholder="Search by school name..." class="form-control">
        </div>
      </div>

      <div class="table-container">
        <div class="loading" *ngIf="isLoading">Loading schools...</div>
        <div class="error-message" *ngIf="errorMessage">{{ errorMessage }}</div>
        
        <table class="schools-table" *ngIf="!isLoading && !errorMessage">
          <thead>
            <tr>
              <th>School Code</th>
              <th>School Name</th>
              <th>District</th>
              <th>Block</th>
              <th>Village</th>
              <th>Type</th>
              <th>Management</th>
              <th>Students</th>
              <th>Teachers</th>
              <th>Principal</th>
              <th>Contact</th>
              <th>Email</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let school of filteredSchools" [class.inactive]="!school.isActive">
              <td>{{ school.schoolCode }}</td>
              <td>{{ school.schoolName }}</td>
              <td>{{ school.district }}</td>
              <td>{{ school.block }}</td>
              <td>{{ school.village }}</td>
              <td>{{ school.schoolType }}</td>
              <td>{{ school.managementType }}</td>
              <td>{{ school.totalStudents }}</td>
              <td>{{ school.totalTeachers }}</td>
              <td>{{ school.principalName }}</td>
              <td>{{ school.contactNumber }}</td>
              <td>{{ school.email }}</td>
              <td>
                <span class="status" [class.active]="school.isActive" [class.inactive]="!school.isActive">
                  {{ school.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td>
                <button (click)="editSchool(school)" class="btn-edit">Edit</button>
                <button (click)="toggleSchoolStatus(school)" class="btn-toggle" 
                        [class.activate]="!school.isActive" [class.deactivate]="school.isActive">
                  {{ school.isActive ? 'Deactivate' : 'Activate' }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Edit Modal -->
      <div class="modal-overlay" *ngIf="showEditModal" (click)="closeEditModal()">
        <div class="modal-content" (click)="$event.stopPropagation()">
          <div class="modal-header">
            <h2>Edit School</h2>
            <button class="close-btn" (click)="closeEditModal()">&times;</button>
          </div>
          <form [formGroup]="editForm" (ngSubmit)="saveSchool()" class="edit-form">
            <div class="form-row">
              <div class="form-group">
                <label for="schoolName">School Name *</label>
                <input type="text" id="schoolName" formControlName="schoolName" class="form-control">
                <div class="error-text" *ngIf="editForm.get('schoolName')?.invalid && editForm.get('schoolName')?.touched">
                  School name is required
                </div>
              </div>
              <div class="form-group">
                <label for="schoolCode">School Code *</label>
                <input type="text" id="schoolCode" formControlName="schoolCode" class="form-control">
                <div class="error-text" *ngIf="editForm.get('schoolCode')?.invalid && editForm.get('schoolCode')?.touched">
                  School code is required
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="district">District *</label>
                <input type="text" id="district" formControlName="district" class="form-control">
              </div>
              <div class="form-group">
                <label for="block">Block *</label>
                <input type="text" id="block" formControlName="block" class="form-control">
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="village">Village *</label>
                <input type="text" id="village" formControlName="village" class="form-control">
              </div>
              <div class="form-group">
                <label for="schoolType">School Type *</label>
                <select id="schoolType" formControlName="schoolType" class="form-control">
                  <option value="Primary">Primary</option>
                  <option value="Middle">Middle</option>
                  <option value="High">High</option>
                  <option value="Senior Secondary">Senior Secondary</option>
                </select>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="managementType">Management Type *</label>
                <select id="managementType" formControlName="managementType" class="form-control">
                  <option value="Government">Government</option>
                  <option value="Aided">Aided</option>
                  <option value="Private">Private</option>
                </select>
              </div>
              <div class="form-group">
                <label for="principalName">Principal Name *</label>
                <input type="text" id="principalName" formControlName="principalName" class="form-control">
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="totalStudents">Total Students</label>
                <input type="number" id="totalStudents" formControlName="totalStudents" class="form-control" min="0">
              </div>
              <div class="form-group">
                <label for="totalTeachers">Total Teachers</label>
                <input type="number" id="totalTeachers" formControlName="totalTeachers" class="form-control" min="0">
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="contactNumber">Contact Number</label>
                <input type="tel" id="contactNumber" formControlName="contactNumber" class="form-control">
              </div>
              <div class="form-group">
                <label for="email">Email</label>
                <input type="email" id="email" formControlName="email" class="form-control">
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="establishedDate">Established Date</label>
                <input type="date" id="establishedDate" formControlName="establishedDate" class="form-control">
              </div>
              <div class="form-group">
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="isActive"> Active
                </label>
              </div>
            </div>

            <div class="form-actions">
              <button type="button" (click)="closeEditModal()" class="btn-cancel">Cancel</button>
              <button type="submit" [disabled]="editForm.invalid || isSaving" class="btn-save">
                {{ isSaving ? 'Saving...' : 'Save Changes' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .school-management-container {
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
    }

    .welcome-text {
      color: #34495e;
      font-weight: 500;
    }

    .btn-logout {
      padding: 8px 16px;
      background-color: #e74c3c;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
    }

    .btn-logout:hover {
      background-color: #c0392b;
    }

    .btn-secondary {
      padding: 8px 16px;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      margin-right: 10px;
    }

    .btn-secondary:hover {
      background-color: #0056b3;
    }

    .filters {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
      padding: 20px;
      background-color: #f8f9fa;
      border-radius: 8px;
    }

    .filter-group {
      display: flex;
      flex-direction: column;
      gap: 5px;
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

    .schools-table {
      width: 100%;
      border-collapse: collapse;
      font-size: 14px;
    }

    .schools-table th {
      background-color: #343a40;
      color: white;
      padding: 12px 8px;
      text-align: left;
      font-weight: 600;
      position: sticky;
      top: 0;
    }

    .schools-table td {
      padding: 10px 8px;
      border-bottom: 1px solid #dee2e6;
      vertical-align: middle;
    }

    .schools-table tr:hover {
      background-color: #f8f9fa;
    }

    .schools-table tr.inactive {
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
      max-width: 800px;
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

    .edit-form {
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
      
      .schools-table {
        font-size: 12px;
      }
      
      .schools-table th,
      .schools-table td {
        padding: 8px 4px;
      }
    }
  `]
})
export class SchoolManagementComponent implements OnInit {
  schools: School[] = [];
  filteredSchools: School[] = [];
  districts: string[] = [];
  selectedDistrict: string = '';
  searchTerm: string = '';
  isLoading = false;
  errorMessage = '';
  userName = '';

  // Edit modal
  showEditModal = false;
  editForm: FormGroup;
  currentSchool: School | null = null;
  isSaving = false;

  constructor(
    private schoolService: SchoolService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.editForm = this.fb.group({
      schoolName: ['', Validators.required],
      schoolCode: ['', Validators.required],
      district: ['', Validators.required],
      block: ['', Validators.required],
      village: ['', Validators.required],
      schoolType: ['', Validators.required],
      managementType: ['', Validators.required],
      totalStudents: [0, [Validators.min(0)]],
      totalTeachers: [0, [Validators.min(0)]],
      principalName: ['', Validators.required],
      contactNumber: [''],
      email: [''],
      establishedDate: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
    this.loadSchools();
  }

  loadSchools() {
    this.isLoading = true;
    this.errorMessage = '';

    this.schoolService.getAllSchools().subscribe({
      next: (schools) => {
        this.schools = schools;
        this.filteredSchools = schools;
        this.extractDistricts();
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load schools. Please try again.';
        this.isLoading = false;
        console.error('Error loading schools:', error);
      }
    });
  }

  extractDistricts() {
    const districtSet = new Set(this.schools.map(school => school.district));
    this.districts = Array.from(districtSet).sort();
  }

  filterByDistrict() {
    this.applyFilters();
  }

  filterSchools() {
    this.applyFilters();
  }

  applyFilters() {
    let filtered = this.schools;

    if (this.selectedDistrict) {
      filtered = filtered.filter(school => school.district === this.selectedDistrict);
    }

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(school => 
        school.schoolName.toLowerCase().includes(term) ||
        school.schoolCode.toLowerCase().includes(term) ||
        school.principalName.toLowerCase().includes(term)
      );
    }

    this.filteredSchools = filtered;
  }

  editSchool(school: School) {
    this.currentSchool = school;
    this.editForm.patchValue({
      schoolName: school.schoolName,
      schoolCode: school.schoolCode,
      district: school.district,
      block: school.block,
      village: school.village,
      schoolType: school.schoolType,
      managementType: school.managementType,
      totalStudents: school.totalStudents,
      totalTeachers: school.totalTeachers,
      principalName: school.principalName,
      contactNumber: school.contactNumber,
      email: school.email,
      establishedDate: school.establishedDate.split('T')[0], // Format date for input
      isActive: school.isActive
    });
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
    this.currentSchool = null;
    this.editForm.reset();
  }

  saveSchool() {
    if (this.editForm.valid && this.currentSchool) {
      this.isSaving = true;
      
      const updateData: UpdateSchool = {
        id: this.currentSchool.id,
        ...this.editForm.value
      };

      this.schoolService.updateSchool(this.currentSchool.id, updateData).subscribe({
        next: (updatedSchool) => {
          const index = this.schools.findIndex(s => s.id === updatedSchool.id);
          if (index !== -1) {
            this.schools[index] = updatedSchool;
            this.applyFilters();
          }
          this.closeEditModal();
          this.isSaving = false;
        },
        error: (error) => {
          console.error('Error updating school:', error);
          this.errorMessage = 'Failed to update school. Please try again.';
          this.isSaving = false;
        }
      });
    }
  }

  toggleSchoolStatus(school: School) {
    const updateData: UpdateSchool = {
      ...school,
      isActive: !school.isActive,
      establishedDate: school.establishedDate.split('T')[0]
    };

    this.schoolService.updateSchool(school.id, updateData).subscribe({
      next: (updatedSchool) => {
        const index = this.schools.findIndex(s => s.id === updatedSchool.id);
        if (index !== -1) {
          this.schools[index] = updatedSchool;
          this.applyFilters();
        }
      },
      error: (error) => {
        console.error('Error toggling school status:', error);
        this.errorMessage = 'Failed to update school status. Please try again.';
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  goToTeachers() {
    this.router.navigate(['/teachers']);
  }

  goToNoticeBoard() {
    this.router.navigate(['/notices']);
  }

  goToAbout() {
    this.router.navigate(['/about']);
  }
}