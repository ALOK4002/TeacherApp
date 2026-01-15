import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserProfileService } from '../../services/user-profile.service';
import { DocumentService } from '../../services/document.service';
import { UserProfile } from '../../models/user-profile.models';
import { TeacherDocument } from '../../models/document.models';
import { of } from 'rxjs';
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
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.userName = this.authService.getUserName() || 'User';
    this.userEmail = this.authService.getUserEmail() || '';
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    let profileLoaded = false;
    let documentsLoaded = false;

    const checkComplete = () => {
      if (profileLoaded && documentsLoaded) {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    };

    // Load profile
    this.userProfileService.getMyProfile().pipe(
      catchError(error => {
        console.error('Error loading profile:', error);
        return of(null);
      })
    ).subscribe(profile => {
      this.userProfile = profile;
      profileLoaded = true;
      checkComplete();
    });

    // Load documents
    this.documentService.getMyDocuments().pipe(
      catchError(error => {
        console.error('Error loading documents:', error);
        return of([] as TeacherDocument[]);
      })
    ).subscribe(documents => {
      this.documents = documents || [];
      documentsLoaded = true;
      checkComplete();
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
