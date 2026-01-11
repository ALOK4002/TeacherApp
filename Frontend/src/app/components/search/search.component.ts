import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SearchService, UnifiedSearchRequest, UnifiedSearchResult } from '../../services/search.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="search-container ms-fadeIn">
      <div class="education-header ms-Card--elevated">
        <div class="ms-Flex ms-Flex--spaceBetween ms-Flex--wrap">
          <div>
            <h1 class="ms-fontSize-32 ms-fontWeight-bold">
              <span class="education-icon">🔍</span>
              Smart Search Portal
            </h1>
            <p class="ms-fontSize-16" style="margin-top: 8px; opacity: 0.9;">
              Search across teachers, schools, and notices with AI-powered insights
            </p>
          </div>
          <div class="ms-Flex ms-Flex--wrap" style="gap: 12px;">
            <span class="knowledge-badge">Welcome, {{ userName }}!</span>
            <button (click)="goToNoticeBoard()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">📢</span> Notice Board
            </button>
            <button (click)="goToTeachers()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">👨‍🏫</span> Teachers
            </button>
            <button (click)="goToSchools()" class="ms-Button ms-Button--secondary">
              <span class="education-icon">🏫</span> Schools
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

      <!-- Search Form -->
      <div class="search-form-section ms-Card ms-Card--elevated">
        <form [formGroup]="searchForm" (ngSubmit)="performSearch()" class="search-form">
          <div class="search-input-group">
            <div class="ms-TextField" style="flex: 1;">
              <input 
                type="text" 
                formControlName="query"
                class="ms-TextField-field search-input"
                placeholder="Search for teachers, schools, or notices..."
                (input)="onSearchInput()"
              >
            </div>
            <button type="submit" class="ms-Button ms-Button--education search-button" [disabled]="isSearching">
              <span class="education-icon" *ngIf="!isSearching">🔍</span>
              <span class="loading-spinner" *ngIf="isSearching">⏳</span>
              {{ isSearching ? 'Searching...' : 'Search' }}
            </button>
          </div>

          <!-- Search Filters -->
          <div class="search-filters ms-Flex ms-Flex--wrap" style="gap: 16px; margin-top: 16px;">
            <div class="filter-group">
              <label class="ms-fontSize-14 ms-fontWeight-medium">Search In:</label>
              <div class="checkbox-group ms-Flex" style="gap: 12px; margin-top: 4px;">
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="searchTeachers"> Teachers
                </label>
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="searchSchools"> Schools
                </label>
                <label class="checkbox-label">
                  <input type="checkbox" formControlName="searchNotices"> Notices
                </label>
              </div>
            </div>

            <div class="filter-group">
              <label class="ms-fontSize-14 ms-fontWeight-medium">Results per page:</label>
              <select formControlName="pageSize" class="ms-TextField-field" style="margin-top: 4px;">
                <option value="10">10</option>
                <option value="20">20</option>
                <option value="50">50</option>
              </select>
            </div>
          </div>
        </form>

        <!-- Search Suggestions -->
        <div class="suggestions-container" *ngIf="suggestions.length > 0">
          <h4 class="ms-fontSize-14 ms-fontWeight-medium">Suggestions:</h4>
          <div class="suggestions-list">
            <button 
              *ngFor="let suggestion of suggestions" 
              (click)="applySuggestion(suggestion)"
              class="suggestion-item ms-Button ms-Button--secondary"
            >
              {{ suggestion }}
            </button>
          </div>
        </div>
      </div>

      <!-- Search Results -->
      <div class="search-results-section" *ngIf="hasSearched">
        <!-- Results Summary -->
        <div class="results-summary ms-Card" *ngIf="searchResults">
          <div class="ms-Flex ms-Flex--spaceBetween ms-Flex--wrap">
            <div>
              <h3 class="ms-fontSize-20 ms-fontWeight-semibold">
                Search Results ({{ searchResults.totalCount }} found)
              </h3>
              <p class="ms-fontSize-14" style="color: var(--neutral-gray-90); margin: 4px 0 0;">
                Showing results for: "{{ lastSearchQuery }}"
              </p>
            </div>
            <div class="type-counts ms-Flex" style="gap: 12px;">
              <span class="knowledge-badge" *ngIf="searchResults.typeCounts['teachers']">
                👨‍🏫 {{ searchResults.typeCounts['teachers'] }} Teachers
              </span>
              <span class="growth-indicator" *ngIf="searchResults.typeCounts['schools']">
                🏫 {{ searchResults.typeCounts['schools'] }} Schools
              </span>
              <span class="creativity-highlight" *ngIf="searchResults.typeCounts['notices']">
                📢 {{ searchResults.typeCounts['notices'] }} Notices
              </span>
            </div>
          </div>
        </div>

        <!-- Loading State -->
        <div class="loading-state ms-Card" *ngIf="isSearching">
          <div class="ms-Flex ms-Flex--center" style="padding: 40px;">
            <span class="loading-spinner" style="font-size: 24px; margin-right: 12px;">⏳</span>
            <span class="ms-fontSize-16">Searching across all data...</span>
          </div>
        </div>

        <!-- No Results -->
        <div class="no-results ms-Card" *ngIf="!isSearching && searchResults && searchResults.results.length === 0">
          <div class="ms-Flex ms-Flex--center ms-Flex--column" style="padding: 40px; text-align: center;">
            <span class="education-icon" style="font-size: 48px; margin-bottom: 16px;">🔍</span>
            <h3 class="ms-fontSize-20 ms-fontWeight-semibold">No results found</h3>
            <p class="ms-fontSize-14" style="color: var(--neutral-gray-90); margin-top: 8px;">
              Try adjusting your search terms or filters
            </p>
          </div>
        </div>

        <!-- Results List -->
        <div class="results-list" *ngIf="!isSearching && searchResults && searchResults.results.length > 0">
          <div 
            *ngFor="let result of searchResults.results" 
            class="result-item ms-Card ms-Card--elevated"
            [class]="'result-' + result.type"
          >
            <div class="result-header">
              <div class="result-type-icon">
                <span *ngIf="result.type === 'teacher'">👨‍🏫</span>
                <span *ngIf="result.type === 'school'">🏫</span>
                <span *ngIf="result.type === 'notice'">📢</span>
              </div>
              <div class="result-info">
                <h4 class="ms-fontSize-16 ms-fontWeight-semibold result-title">
                  {{ result.title }}
                </h4>
                <p class="ms-fontSize-14 result-description">
                  {{ result.description }}
                </p>
                <div class="result-meta ms-Flex" style="gap: 12px; margin-top: 8px;">
                  <span class="result-category">{{ result.category }}</span>
                  <span class="result-date">{{ result.date | date:'short' }}</span>
                  <span class="result-type">{{ result.type | titlecase }}</span>
                </div>
              </div>
            </div>
            <div class="result-actions">
              <button 
                (click)="viewDetails(result)" 
                class="ms-Button ms-Button--secondary"
              >
                View Details
              </button>
            </div>
          </div>
        </div>

        <!-- Pagination -->
        <div class="pagination-section ms-Card" *ngIf="searchResults && searchResults.results.length > 0">
          <div class="ms-Flex ms-Flex--spaceBetween ms-Flex--center">
            <span class="ms-fontSize-14">
              Showing {{ (currentPage - 1) * pageSize + 1 }} - 
              {{ Math.min(currentPage * pageSize, searchResults.totalCount) }} 
              of {{ searchResults.totalCount }} results
            </span>
            <div class="pagination-controls ms-Flex" style="gap: 8px;">
              <button 
                (click)="previousPage()" 
                [disabled]="currentPage <= 1"
                class="ms-Button ms-Button--secondary"
              >
                Previous
              </button>
              <span class="ms-fontSize-14" style="padding: 8px 12px;">
                Page {{ currentPage }} of {{ totalPages }}
              </span>
              <button 
                (click)="nextPage()" 
                [disabled]="currentPage >= totalPages"
                class="ms-Button ms-Button--secondary"
              >
                Next
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Admin Section -->
      <div class="admin-section ms-Card" *ngIf="showAdminSection">
        <h3 class="ms-fontSize-18 ms-fontWeight-semibold">
          <span class="education-icon">⚙️</span>
          Search Administration
        </h3>
        <div class="admin-actions ms-Flex ms-Flex--wrap" style="gap: 12px; margin-top: 16px;">
          <button (click)="initializeIndexes()" class="ms-Button ms-Button--warning" [disabled]="isAdminAction">
            Initialize Indexes
          </button>
          <button (click)="syncAllData()" class="ms-Button ms-Button--success" [disabled]="isAdminAction">
            Sync All Data
          </button>
          <button (click)="getSearchStats()" class="ms-Button ms-Button--secondary" [disabled]="isAdminAction">
            View Statistics
          </button>
        </div>
        <div class="admin-status" *ngIf="adminMessage">
          <p class="ms-fontSize-14" [style.color]="adminMessageType === 'error' ? 'var(--error-color)' : 'var(--success-color)'">
            {{ adminMessage }}
          </p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .search-container {
      padding: var(--spacing-xxl);
      max-width: 1200px;
      margin: 0 auto;
      min-height: 100vh;
    }

    .search-form-section {
      margin: var(--spacing-xxl) 0;
      padding: var(--spacing-xxl);
    }

    .search-input-group {
      display: flex;
      gap: var(--spacing-m);
      align-items: flex-end;
    }

    .search-input {
      font-size: var(--font-size-16) !important;
      padding: var(--spacing-m) !important;
      min-height: 48px;
    }

    .search-button {
      min-height: 48px;
      padding: 0 var(--spacing-xxl);
    }

    .search-filters {
      padding-top: var(--spacing-l);
      border-top: 1px solid var(--neutral-gray-30);
    }

    .filter-group {
      display: flex;
      flex-direction: column;
    }

    .checkbox-group {
      margin-top: var(--spacing-xs);
    }

    .checkbox-label {
      display: flex;
      align-items: center;
      gap: var(--spacing-xs);
      font-size: var(--font-size-14);
      cursor: pointer;
    }

    .suggestions-container {
      margin-top: var(--spacing-l);
      padding-top: var(--spacing-l);
      border-top: 1px solid var(--neutral-gray-30);
    }

    .suggestions-list {
      display: flex;
      flex-wrap: wrap;
      gap: var(--spacing-s);
      margin-top: var(--spacing-s);
    }

    .suggestion-item {
      font-size: var(--font-size-12);
      padding: var(--spacing-xs) var(--spacing-s);
    }

    .search-results-section {
      margin-top: var(--spacing-xxl);
    }

    .results-summary {
      padding: var(--spacing-l);
      margin-bottom: var(--spacing-l);
    }

    .type-counts {
      flex-wrap: wrap;
    }

    .results-list {
      display: flex;
      flex-direction: column;
      gap: var(--spacing-l);
    }

    .result-item {
      padding: var(--spacing-l);
      transition: all 0.2s ease-in-out;
    }

    .result-item:hover {
      transform: translateY(-2px);
      box-shadow: var(--shadow-16);
    }

    .result-item.result-teacher {
      border-left: 4px solid var(--accent-education);
    }

    .result-item.result-school {
      border-left: 4px solid var(--accent-growth);
    }

    .result-item.result-notice {
      border-left: 4px solid var(--accent-creativity);
    }

    .result-header {
      display: flex;
      gap: var(--spacing-m);
      margin-bottom: var(--spacing-m);
    }

    .result-type-icon {
      font-size: var(--font-size-24);
      flex-shrink: 0;
    }

    .result-info {
      flex: 1;
    }

    .result-title {
      color: var(--neutral-gray-120);
      margin: 0 0 var(--spacing-xs);
    }

    .result-description {
      color: var(--neutral-gray-90);
      margin: 0 0 var(--spacing-s);
      line-height: 1.5;
    }

    .result-meta {
      flex-wrap: wrap;
    }

    .result-category,
    .result-date,
    .result-type {
      font-size: var(--font-size-12);
      padding: var(--spacing-xs) var(--spacing-s);
      background-color: var(--neutral-gray-20);
      border-radius: var(--border-radius-small);
      color: var(--neutral-gray-90);
    }

    .result-actions {
      display: flex;
      justify-content: flex-end;
    }

    .pagination-section {
      padding: var(--spacing-l);
      margin-top: var(--spacing-l);
    }

    .pagination-controls {
      align-items: center;
    }

    .admin-section {
      padding: var(--spacing-l);
      margin-top: var(--spacing-xxl);
      background: var(--neutral-gray-10);
    }

    .admin-actions {
      margin-top: var(--spacing-m);
    }

    .admin-status {
      margin-top: var(--spacing-m);
      padding: var(--spacing-s);
      background: var(--neutral-gray-20);
      border-radius: var(--border-radius-medium);
    }

    .loading-spinner {
      animation: spin 1s linear infinite;
    }

    @keyframes spin {
      from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
    }

    /* Responsive Design */
    @media (max-width: 768px) {
      .search-container {
        padding: var(--spacing-l);
      }

      .search-input-group {
        flex-direction: column;
        align-items: stretch;
      }

      .search-filters {
        flex-direction: column;
        gap: var(--spacing-m);
      }

      .result-header {
        flex-direction: column;
        gap: var(--spacing-s);
      }

      .education-header .ms-Flex {
        flex-direction: column;
        align-items: flex-start;
        gap: var(--spacing-l);
      }
    }
  `]
})
export class SearchComponent implements OnInit {
  searchForm: FormGroup;
  searchResults: any = null;
  suggestions: string[] = [];
  isSearching = false;
  hasSearched = false;
  lastSearchQuery = '';
  userName = '';
  showAdminSection = false;
  isAdminAction = false;
  adminMessage = '';
  adminMessageType: 'success' | 'error' = 'success';

  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalPages = 1;

  // Math reference for template
  Math = Math;

  constructor(
    private searchService: SearchService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.searchForm = this.fb.group({
      query: [''],
      searchTeachers: [true],
      searchSchools: [true],
      searchNotices: [true],
      pageSize: [20]
    });
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
    
    // Enable admin section for demonstration
    this.showAdminSection = true;

    // Watch for page size changes
    this.searchForm.get('pageSize')?.valueChanges.subscribe(value => {
      this.pageSize = parseInt(value);
      if (this.hasSearched) {
        this.performSearch();
      }
    });
  }

  onSearchInput() {
    const query = this.searchForm.get('query')?.value;
    if (query && query.length > 2) {
      this.searchService.getSuggestions(query).subscribe({
        next: (suggestions) => {
          this.suggestions = suggestions.slice(0, 5);
        },
        error: (error) => {
          console.error('Error getting suggestions:', error);
        }
      });
    } else {
      this.suggestions = [];
    }
  }

  applySuggestion(suggestion: string) {
    this.searchForm.patchValue({ query: suggestion });
    this.suggestions = [];
    this.performSearch();
  }

  performSearch() {
    const formValue = this.searchForm.value;
    if (!formValue.query || formValue.query.trim() === '') {
      return;
    }

    this.isSearching = true;
    this.hasSearched = true;
    this.lastSearchQuery = formValue.query;
    this.currentPage = 1;

    const searchTypes = [];
    if (formValue.searchTeachers) searchTypes.push('teachers');
    if (formValue.searchSchools) searchTypes.push('schools');
    if (formValue.searchNotices) searchTypes.push('notices');

    const request: UnifiedSearchRequest = {
      query: formValue.query,
      searchTypes: searchTypes,
      skip: (this.currentPage - 1) * this.pageSize,
      top: this.pageSize
    };

    this.searchService.unifiedSearch(request).subscribe({
      next: (results) => {
        this.searchResults = results;
        this.totalPages = Math.ceil(results.totalCount / this.pageSize);
        this.isSearching = false;
        this.suggestions = [];
      },
      error: (error) => {
        console.error('Search error:', error);
        this.isSearching = false;
        // Handle error - maybe show error message
      }
    });
  }

  viewDetails(result: UnifiedSearchResult) {
    // Navigate to appropriate detail page based on result type
    switch (result.type) {
      case 'teacher':
        // Navigate to teacher details or show modal
        console.log('View teacher details:', result);
        break;
      case 'school':
        // Navigate to school details or show modal
        console.log('View school details:', result);
        break;
      case 'notice':
        // Navigate to notice details or show modal
        console.log('View notice details:', result);
        break;
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.performSearch();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.performSearch();
    }
  }

  // Admin functions
  initializeIndexes() {
    this.isAdminAction = true;
    this.adminMessage = '';

    this.searchService.initializeIndexes().subscribe({
      next: (response) => {
        this.adminMessage = 'Search indexes initialized successfully!';
        this.adminMessageType = 'success';
        this.isAdminAction = false;
      },
      error: (error) => {
        this.adminMessage = 'Failed to initialize indexes: ' + (error.error?.message || error.message);
        this.adminMessageType = 'error';
        this.isAdminAction = false;
      }
    });
  }

  syncAllData() {
    this.isAdminAction = true;
    this.adminMessage = '';

    this.searchService.syncData().subscribe({
      next: (response) => {
        this.adminMessage = 'All data synced successfully!';
        this.adminMessageType = 'success';
        this.isAdminAction = false;
      },
      error: (error) => {
        this.adminMessage = 'Failed to sync data: ' + (error.error?.message || error.message);
        this.adminMessageType = 'error';
        this.isAdminAction = false;
      }
    });
  }

  getSearchStats() {
    this.isAdminAction = true;
    this.adminMessage = '';

    this.searchService.getSearchStats().subscribe({
      next: (stats) => {
        this.adminMessage = `Search Statistics: Teachers Index: ${stats.teachersIndexExists ? 'Exists' : 'Missing'}, Schools Index: ${stats.schoolsIndexExists ? 'Exists' : 'Missing'}, Notices Index: ${stats.noticesIndexExists ? 'Exists' : 'Missing'}`;
        this.adminMessageType = 'success';
        this.isAdminAction = false;
      },
      error: (error) => {
        this.adminMessage = 'Failed to get statistics: ' + (error.error?.message || error.message);
        this.adminMessageType = 'error';
        this.isAdminAction = false;
      }
    });
  }

  // Navigation methods
  goToNoticeBoard() {
    this.router.navigate(['/notices']);
  }

  goToTeachers() {
    this.router.navigate(['/teachers']);
  }

  goToSchools() {
    this.router.navigate(['/schools']);
  }

  goToAbout() {
    this.router.navigate(['/about']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}