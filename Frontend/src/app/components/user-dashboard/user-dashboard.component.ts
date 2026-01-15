import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserProfileService } from '../../services/user-profile.service';
import { DocumentService } from '../../services/document.service';
import { UserProfile } from '../../models/user-profile.models';
import { TeacherDocument } from '../../models/document.models';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit {
  userName = '';
  userEmail = '';
  userProfile: UserProfile | null = null;
  documents: TeacherDocument[] = [];
  isLoading = true;

  constructor(
    private authService: AuthService,
    private userProfileService: UserProfileService,
    private documentService: DocumentService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userName = this.authService.getUserName() || 'User';
    this.userEmail = this.authService.getUserEmail() || '';
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    
    // Load both profile and documents in parallel
    forkJoin({
      profile: this.userProfileService.getMyProfile().pipe(
        catchError(error => {
          console.error('Error loading profile:', error);
          return of(null);
        })
      ),
      documents: this.documentService.getMyDocuments().pipe(
        catchError(error => {
          console.error('Error loading documents:', error);
          return of([]);
        })
      )
    }).subscribe({
      next: (result) => {
        this.userProfile = result.profile;
        this.documents = result.documents || [];
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading data:', error);
        this.isLoading = false;
      }
    });
  }

  loadProfile() {
    this.userProfileService.getMyProfile().subscribe({
      next: (profile) => {
        this.userProfile = profile;
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.userProfile = null;
      }
    });
  }

  loadDocuments() {
    this.documentService.getMyDocuments().subscribe({
      next: (documents) => {
        this.documents = documents;
      },
      error: (error) => {
        console.error('Error loading documents:', error);
      }
    });
  }

  navigateToDocuments() {
    this.router.navigate(['/my-documents']);
  }

  navigateToEditProfile() {
    this.router.navigate(['/self-declaration']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
