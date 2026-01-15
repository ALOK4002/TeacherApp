import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserProfileService } from '../../services/user-profile.service';
import { DocumentService } from '../../services/document.service';
import { UserProfile } from '../../models/user-profile.models';
import { TeacherDocument } from '../../models/document.models';

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
    this.loadProfile();
    this.loadDocuments();
  }

  loadProfile() {
    this.userProfileService.getMyProfile().subscribe({
      next: (profile) => {
        this.userProfile = profile;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.isLoading = false;
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
