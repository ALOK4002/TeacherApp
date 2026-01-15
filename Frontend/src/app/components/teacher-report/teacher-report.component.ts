import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TeacherService } from '../../services/teacher.service';
import { AuthService } from '../../services/auth.service';
import { TeacherReport, TeacherReportSearchRequest, PagedResult } from '../../models/teacher.models';

@Component({
  selector: 'app-teacher-report',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="teacher-report-container ms-fadeIn">
      <div class="education-header ms-Card--elevated">
        <div class="ms-Flex ms-Flex--spaceBetween ms-Flex--wrap">
          <div>
            <h1 class="ms-fontSize-32 ms-fontWeight-bold">
              <span class="education-icon">📊</span>
              Teacher-School Report
            </h1>
            <p class="ms-fontSize-16" style="margin-top: 8px; opacity: 0.9;">
              Comprehensive report of teachers with school information, search and pagination
            </p>
          </div>
          <div class="ms-Flex ms-Flex--wrap" style="gap: 12px;">
            <span class="knowledge-badge">Welcome, {{ userName }}!</span>
            <button (click)="goToTeachers()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">👨‍🏫</span> Manage Teachers
            </button>
            <button (click)="goToSchools()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">🏫</span> Schools
            </button>
            <button (click)="goToNoticeBoard()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">📢</span> Notice Board
            </button>
            <button (click)="goToAbout()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">ℹ️</span> About
            </button>
            <button (click)="logout()" class="ms-Button ms-Button--danger">
              <span class="education-icon">🚪</span> Logout
            </button>
          </div>
        </div>
      </div>

      <!-- Search and Filter Section -->
      <div class="search-section ms-Card ms-Card--elevated">
        <form [formGroup]="searchForm" (ngSubmit)="searchTeachers()" class="search-form">
          <div class="search-row">
            <div class="ms-TextField search-field">
              <label class="ms-Label">Global Search</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="searchTerm"
                placeholder="Search across all fields..."
              />
            </div>
            <div class="ms-TextField search-field">
              <label class="ms-Label">Teacher Name</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="teacherName"
                placeholder="Filter by teacher name..."
              />
            </div>
            <div class="ms-TextField search-field">
              <label class="ms-Label">School Name</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="schoolName"
                placeholder="Filter by school name..."
              />
            </div>
          </div>

          <div class="search-row">
            <div class="ms-TextField search-field">
              <label class="ms-Label">District</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="district"
                placeholder="Filter by district..."
              />
            </div>
            <div class="ms-TextField search-field">
              <label class="ms-Label">Pincode</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="pincode"
                placeholder="Filter by pincode..."
              />
            </div>
            <div class="ms-TextField search-field">
              <label class="ms-Label">Contact Number</label>
              <input 
                type="text" 
                class="ms-TextField-field" 
                formControlName="contactNumber"
                placeholder="Filter by contact number..."
              />
            </div>
          </div>

          <div class="search-actions">
            <button type="submit" class="ms-Button ms-Button--primary" [disabled]="loading">
              <span class="education-icon">🔍</span>
              {{ loading ? 'Searching...' : 'Search' }}
            </button>
            <button type="button" (click)="clearSearch()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">🗑️</span> Clear
            </button>
            <div class="page-size-selector">
              <label class="ms-Label">Records per page:</label>
              <select class="ms-Dropdown-select" [(ngModel)]="pageSize" (change)="onPageSizeChange()" [ngModelOptions]="{standalone: true}">
                <option value="20">20</option>
                <option value="50">50</option>
                <option value="100">100</option>
              </select>
            </div>
          </div>
        </form>
      </div>

      <!-- Results Section -->
      <div class="results-section ms-Card ms-Card--elevated" *ngIf="!loading">
        <div class="results-header">
          <h3 class="ms-fontSize-20 ms-fontWeight-semibold">
            <span class="education-icon">📋</span>
            Teacher Report Results
          </h3>
          <div class="results-info">
            <span class="ms-fontSize-14">
              Showing {{ getStartRecord() }}-{{ getEndRecord() }} of {{ pagedResult?.totalCount || 0 }} teachers
            </span>
          </div>
        </div>

        <!-- Results Table -->
        <div class="table-container" *ngIf="pagedResult && pagedResult.items.length > 0">
          <table class="ms-Table">
            <thead>
              <tr class="ms-Table-row">
                <th class="ms-Table-cell sortable" (click)="sort('teacherName')">
                  Teacher Name
                  <span class="sort-indicator" [ngClass]="getSortClass('teacherName')">{{ getSortIcon('teacherName') }}</span>
                </th>
                <th class="ms-Table-cell sortable" (click)="sort('schoolName')">
                  School Name
                  <span class="sort-indicator" [ngClass]="getSortClass('schoolName')">{{ getSortIcon('schoolName') }}</span>
                </th>
                <th class="ms-Table-cell sortable" (click)="sort('district')">
                  District
                  <span class="sort-indicator" [ngClass]="getSortClass('district')">{{ getSortIcon('district') }}</span>
                </th>
                <th class="ms-Table-cell sortable" (click)="sort('pincode')">
                  Pincode
                  <span class="sort-indicator" [ngClass]="getSortClass('pincode')">{{ getSortIcon('pincode') }}</span>
                </th>
                <th class="ms-Table-cell sortable" (click)="sort('contactNumber')">
                  Contact Number
                  <span class="sort-indicator" [ngClass]="getSortClass('contactNumber')">{{ getSortIcon('contactNumber') }}</span>
                </th>
                <th class="ms-Table-cell">Email</th>
                <th class="ms-Table-cell">Class/Subject</th>
                <th class="ms-Table-cell">Status</th>
              </tr>
            </thead>
            <tbody>
              <tr class="ms-Table-row" *ngFor="let teacher of pagedResult.items; trackBy: trackByTeacherId">
                <td class="ms-Table-cell">
                  <div class="teacher-info">
                    <span class="teacher-name">{{ teacher.teacherName }}</span>
                    <small class="teacher-address">{{ teacher.address }}</small>
                  </div>
                </td>
                <td class="ms-Table-cell">
                  <span class="school-name">{{ teacher.schoolName }}</span>
                </td>
                <td class="ms-Table-cell">
                  <span class="district-badge">{{ teacher.district }}</span>
                </td>
                <td class="ms-Table-cell">
                  <span class="pincode">{{ teacher.pincode }}</span>
                </td>
                <td class="ms-Table-cell">
                  <span class="contact-number">{{ teacher.contactNumber }}</span>
                </td>
                <td class="ms-Table-cell">
                  <span class="email">{{ teacher.email }}</span>
                </td>
                <td class="ms-Table-cell">
                  <div class="class-subject">
                    <span class="class">{{ teacher.classTeaching }}</span>
                    <small class="subject">{{ teacher.subject }}</small>
                  </div>
                </td>
                <td class="ms-Table-cell">
                  <span class="status-badge" [ngClass]="teacher.isActive ? 'active' : 'inactive'">
                    {{ teacher.isActive ? 'Active' : 'Inactive' }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- No Results -->
        <div class="no-results" *ngIf="pagedResult && pagedResult.items.length === 0">
          <div class="no-results-content">
            <span class="education-icon large">📭</span>
            <h3>No Teachers Found</h3>
            <p>Try adjusting your search criteria or clear filters to see all teachers.</p>
          </div>
        </div>

        <!-- Pagination -->
        <div class="pagination-section" *ngIf="pagedResult && pagedResult.totalPages > 1">
          <div class="pagination-info">
            <span class="ms-fontSize-14">
              Page {{ pagedResult.page }} of {{ pagedResult.totalPages }}
            </span>
          </div>
          <div class="pagination-controls">
            <button 
              class="ms-Button ms-Button--secondary" 
              [disabled]="!pagedResult.hasPreviousPage"
              (click)="goToPage(1)">
              <span class="education-icon">⏮️</span> First
            </button>
            <button 
              class="ms-Button ms-Button--secondary" 
              [disabled]="!pagedResult.hasPreviousPage"
              (click)="goToPage(pagedResult.page - 1)">
              <span class="education-icon">⬅️</span> Previous
            </button>
            <span class="page-numbers">
              <button 
                *ngFor="let page of getVisiblePages()" 
                class="ms-Button page-button"
                [ngClass]="{'ms-Button--primary': page === pagedResult.page, 'ms-Button--secondary': page !== pagedResult.page}"
                (click)="goToPage(page)">
                {{ page }}
              </button>
            </span>
            <button 
              class="ms-Button ms-Button--secondary" 
              [disabled]="!pagedResult.hasNextPage"
              (click)="goToPage(pagedResult.page + 1)">
              Next <span class="education-icon">➡️</span>
            </button>
            <button 
              class="ms-Button ms-Button--secondary" 
              [disabled]="!pagedResult.hasNextPage"
              (click)="goToPage(pagedResult.totalPages)">
              Last <span class="education-icon">⏭️</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      <div class="loading-section ms-Card ms-Card--elevated" *ngIf="loading">
        <div class="loading-content">
          <div class="ms-Spinner ms-Spinner--large"></div>
          <p class="ms-fontSize-16">Loading teacher report...</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .teacher-report-container {
      padding: var(--spacing-xl);
      max-width: 1400px;
      margin: 0 auto;
      background: var(--neutral-gray-10);
      min-height: 100vh;
    }

    .education-header {
      background: var(--gradient-education);
      color: var(--neutral-white);
      padding: var(--spacing-xl);
      border-radius: var(--border-radius-large);
      margin-bottom: var(--spacing-xl);
      box-shadow: var(--shadow-16);
    }

    .education-icon {
      margin-right: var(--spacing-s);
    }

    .education-icon.large {
      font-size: 48px;
      margin-bottom: var(--spacing-m);
    }

    .knowledge-badge {
      background: var(--accent-knowledge);
      color: var(--neutral-white);
      padding: var(--spacing-s) var(--spacing-m);
      border-radius: var(--border-radius-medium);
      font-size: var(--font-size-14);
      font-weight: var(--font-weight-medium);
    }

    .search-section {
      background: var(--neutral-white);
      padding: var(--spacing-xl);
      border-radius: var(--border-radius-large);
      margin-bottom: var(--spacing-xl);
      box-shadow: var(--shadow-4);
    }

    .search-form {
      display: flex;
      flex-direction: column;
      gap: var(--spacing-l);
    }

    .search-row {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: var(--spacing-l);
    }

    .search-field {
      display: flex;
      flex-direction: column;
    }

    .ms-Label {
      font-weight: var(--font-weight-medium);
      margin-bottom: var(--spacing-xs);
      color: var(--neutral-gray-120);
    }

    .ms-TextField-field {
      padding: var(--spacing-s) var(--spacing-m);
      border: 1px solid var(--neutral-gray-60);
      border-radius: var(--border-radius-medium);
      font-size: var(--font-size-14);
      transition: border-color 0.2s ease;
    }

    .ms-TextField-field:focus {
      outline: none;
      border-color: var(--primary-color);
      box-shadow: 0 0 0 1px var(--primary-color);
    }

    .search-actions {
      display: flex;
      align-items: center;
      gap: var(--spacing-m);
      flex-wrap: wrap;
    }

    .page-size-selector {
      display: flex;
      align-items: center;
      gap: var(--spacing-s);
      margin-left: auto;
    }

    .ms-Dropdown-select {
      padding: var(--spacing-s);
      border: 1px solid var(--neutral-gray-60);
      border-radius: var(--border-radius-medium);
      font-size: var(--font-size-14);
    }

    .results-section {
      background: var(--neutral-white);
      border-radius: var(--border-radius-large);
      box-shadow: var(--shadow-4);
      overflow: hidden;
    }

    .results-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: var(--spacing-xl);
      background: var(--neutral-gray-20);
      border-bottom: 1px solid var(--neutral-gray-40);
    }

    .results-info {
      color: var(--neutral-gray-90);
    }

    .table-container {
      overflow-x: auto;
    }

    .ms-Table {
      width: 100%;
      border-collapse: collapse;
    }

    .ms-Table-row {
      border-bottom: 1px solid var(--neutral-gray-30);
    }

    .ms-Table-row:hover {
      background: var(--neutral-gray-10);
    }

    .ms-Table-cell {
      padding: var(--spacing-m);
      text-align: left;
      vertical-align: top;
    }

    .ms-Table thead .ms-Table-cell {
      background: var(--neutral-gray-20);
      font-weight: var(--font-weight-semibold);
      color: var(--neutral-gray-120);
      position: sticky;
      top: 0;
      z-index: 1;
    }

    .sortable {
      cursor: pointer;
      user-select: none;
      position: relative;
    }

    .sortable:hover {
      background: var(--neutral-gray-30);
    }

    .sort-indicator {
      margin-left: var(--spacing-xs);
      font-size: var(--font-size-12);
    }

    .sort-indicator.asc {
      color: var(--primary-color);
    }

    .sort-indicator.desc {
      color: var(--primary-color);
    }

    .teacher-info {
      display: flex;
      flex-direction: column;
    }

    .teacher-name {
      font-weight: var(--font-weight-medium);
      color: var(--neutral-gray-120);
    }

    .teacher-address {
      color: var(--neutral-gray-90);
      font-size: var(--font-size-12);
      margin-top: 2px;
    }

    .school-name {
      font-weight: var(--font-weight-medium);
      color: var(--accent-education);
    }

    .district-badge {
      background: var(--accent-knowledge);
      color: var(--neutral-white);
      padding: 2px var(--spacing-s);
      border-radius: var(--border-radius-small);
      font-size: var(--font-size-12);
      font-weight: var(--font-weight-medium);
    }

    .class-subject {
      display: flex;
      flex-direction: column;
    }

    .class {
      font-weight: var(--font-weight-medium);
      color: var(--neutral-gray-120);
    }

    .subject {
      color: var(--neutral-gray-90);
      font-size: var(--font-size-12);
      margin-top: 2px;
    }

    .status-badge {
      padding: 2px var(--spacing-s);
      border-radius: var(--border-radius-small);
      font-size: var(--font-size-12);
      font-weight: var(--font-weight-medium);
    }

    .status-badge.active {
      background: var(--success-light);
      color: var(--success-color);
    }

    .status-badge.inactive {
      background: var(--error-light);
      color: var(--error-color);
    }

    .no-results {
      padding: var(--spacing-xxxl);
      text-align: center;
    }

    .no-results-content h3 {
      margin: var(--spacing-m) 0;
      color: var(--neutral-gray-120);
    }

    .no-results-content p {
      color: var(--neutral-gray-90);
    }

    .pagination-section {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: var(--spacing-xl);
      background: var(--neutral-gray-10);
      border-top: 1px solid var(--neutral-gray-40);
    }

    .pagination-controls {
      display: flex;
      align-items: center;
      gap: var(--spacing-s);
    }

    .page-numbers {
      display: flex;
      gap: 2px;
      margin: 0 var(--spacing-m);
    }

    .page-button {
      min-width: 40px;
      height: 32px;
      padding: 0;
    }

    .loading-section {
      background: var(--neutral-white);
      padding: var(--spacing-xxxl);
      border-radius: var(--border-radius-large);
      box-shadow: var(--shadow-4);
      text-align: center;
    }

    .loading-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: var(--spacing-l);
    }

    .ms-Button {
      padding: var(--spacing-s) var(--spacing-l);
      border: none;
      border-radius: var(--border-radius-medium);
      font-size: var(--font-size-14);
      font-weight: var(--font-weight-medium);
      cursor: pointer;
      transition: all 0.2s ease;
      display: inline-flex;
      align-items: center;
      gap: var(--spacing-xs);
    }

    .ms-Button--primary {
      background: var(--primary-color);
      color: var(--neutral-white);
    }

    .ms-Button--primary:hover:not(:disabled) {
      background: var(--primary-hover);
    }

    .ms-Button--secondary {
      background: var(--neutral-gray-20);
      color: var(--neutral-gray-120);
      border: 1px solid var(--neutral-gray-60);
    }

    .ms-Button--secondary:hover:not(:disabled) {
      background: var(--neutral-gray-30);
    }

    .ms-Button--danger {
      background: var(--error-color);
      color: var(--neutral-white);
    }

    .ms-Button--danger:hover:not(:disabled) {
      background: #b71c1c;
    }

    .ms-Button:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }

    .ms-Card {
      background: var(--neutral-white);
      border-radius: var(--border-radius-large);
      box-shadow: var(--shadow-2);
    }

    .ms-Card--elevated {
      box-shadow: var(--shadow-8);
    }

    .ms-fadeIn {
      animation: fadeIn 0.3s ease-in;
    }

    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(20px); }
      to { opacity: 1; transform: translateY(0); }
    }

    .ms-Spinner {
      width: 32px;
      height: 32px;
      border: 3px solid var(--neutral-gray-40);
      border-top: 3px solid var(--primary-color);
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }

    .ms-Spinner--large {
      width: 48px;
      height: 48px;
    }

    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }

    @media (max-width: 768px) {
      .teacher-report-container {
        padding: var(--spacing-m);
      }

      .education-header {
        padding: var(--spacing-l);
      }

      .education-header .ms-Flex {
        flex-direction: column;
        gap: var(--spacing-m);
      }

      .search-row {
        grid-template-columns: 1fr;
      }

      .search-actions {
        flex-direction: column;
        align-items: stretch;
      }

      .page-size-selector {
        margin-left: 0;
        justify-content: center;
      }

      .pagination-section {
        flex-direction: column;
        gap: var(--spacing-m);
      }

      .pagination-controls {
        flex-wrap: wrap;
        justify-content: center;
      }

      .page-numbers {
        margin: 0;
      }
    }
  `]
})
export class TeacherReportComponent implements OnInit {
  searchForm: FormGroup;
  pagedResult: PagedResult<TeacherReport> | null = null;
  loading = false;
  userName: string | null = null;
  pageSize = 20;
  currentSortBy = 'teacherName';
  currentSortDirection = 'asc';

  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private authService: AuthService,
    private router: Router
  ) {
    this.searchForm = this.fb.group({
      searchTerm: [''],
      teacherName: [''],
      schoolName: [''],
      district: [''],
      pincode: [''],
      contactNumber: ['']
    });
  }

  ngOnInit(): void {
    this.userName = this.authService.getUserName();
    this.loadTeachers();
  }

  loadTeachers(page: number = 1): void {
    this.loading = true;
    
    const formValue = this.searchForm.value;
    const request: TeacherReportSearchRequest = {
      searchTerm: formValue.searchTerm || undefined,
      teacherName: formValue.teacherName || undefined,
      schoolName: formValue.schoolName || undefined,
      district: formValue.district || undefined,
      pincode: formValue.pincode || undefined,
      contactNumber: formValue.contactNumber || undefined,
      page: page,
      pageSize: this.pageSize,
      sortBy: this.currentSortBy,
      sortDirection: this.currentSortDirection
    };

    this.teacherService.getTeacherReport(request).subscribe({
      next: (result) => {
        this.pagedResult = result;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading teacher report:', error);
        this.loading = false;
      }
    });
  }

  searchTeachers(): void {
    this.loadTeachers(1);
  }

  clearSearch(): void {
    this.searchForm.reset();
    this.loadTeachers(1);
  }

  onPageSizeChange(): void {
    this.loadTeachers(1);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= (this.pagedResult?.totalPages || 1)) {
      this.loadTeachers(page);
    }
  }

  sort(column: string): void {
    if (this.currentSortBy === column) {
      this.currentSortDirection = this.currentSortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.currentSortBy = column;
      this.currentSortDirection = 'asc';
    }
    this.loadTeachers(this.pagedResult?.page || 1);
  }

  getSortClass(column: string): string {
    if (this.currentSortBy === column) {
      return this.currentSortDirection;
    }
    return '';
  }

  getSortIcon(column: string): string {
    if (this.currentSortBy === column) {
      return this.currentSortDirection === 'asc' ? '↑' : '↓';
    }
    return '';
  }

  getVisiblePages(): number[] {
    if (!this.pagedResult) return [];
    
    const totalPages = this.pagedResult.totalPages;
    const currentPage = this.pagedResult.page;
    const pages: number[] = [];
    
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  getStartRecord(): number {
    if (!this.pagedResult || this.pagedResult.items.length === 0) return 0;
    return (this.pagedResult.page - 1) * this.pagedResult.pageSize + 1;
  }

  getEndRecord(): number {
    if (!this.pagedResult || this.pagedResult.items.length === 0) return 0;
    return Math.min(
      this.pagedResult.page * this.pagedResult.pageSize,
      this.pagedResult.totalCount
    );
  }

  trackByTeacherId(index: number, teacher: TeacherReport): number {
    return teacher.id;
  }

  // Navigation methods
  goToTeachers(): void {
    this.router.navigate(['/teacher-management']);
  }

  goToSchools(): void {
    this.router.navigate(['/school-management']);
  }

  goToNoticeBoard(): void {
    this.router.navigate(['/notice-board']);
  }

  goToAbout(): void {
    this.router.navigate(['/about']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}