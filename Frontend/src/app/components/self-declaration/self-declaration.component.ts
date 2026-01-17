import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserProfileService } from '../../services/user-profile.service';
import { SchoolService } from '../../services/school.service';
import { TeacherService } from '../../services/teacher.service';
import { School } from '../../models/school.models';
import { District } from '../../models/teacher.models';

@Component({
  selector: 'app-self-declaration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './self-declaration.component.html',
  styleUrls: ['./self-declaration.component.css']
})
export class SelfDeclarationComponent implements OnInit {
  profileForm: FormGroup;
  schools: School[] = [];
  districts: District[] = [];
  pincodes: string[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  userEmail = '';
  profileId: number | null = null;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userProfileService: UserProfileService,
    private schoolService: SchoolService,
    private teacherService: TeacherService,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      teacherName: ['', [Validators.required, Validators.minLength(3)]],
      address: ['', [Validators.required]],
      district: ['', [Validators.required]],
      pincode: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
      schoolId: ['', [Validators.required]],
      classTeaching: ['', [Validators.required]],
      subject: ['', [Validators.required]],
      qualification: ['', [Validators.required]],
      contactNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      email: ['', [Validators.required, Validators.email]],
      dateOfJoining: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.userEmail = this.authService.getUserEmail() || '';
    this.profileForm.patchValue({ email: this.userEmail });
    this.loadDistricts();
    this.loadSchools();
    this.loadMyProfile();
  }

  private loadMyProfile() {
    this.userProfileService.getMyProfile().subscribe({
      next: (profile) => {
        this.profileId = profile.id;
        this.isEditMode = true;

        this.profileForm.patchValue({
          teacherName: profile.teacherName,
          address: profile.address,
          district: profile.district,
          pincode: profile.pincode,
          schoolId: profile.schoolId?.toString(),
          classTeaching: profile.classTeaching,
          subject: profile.subject,
          qualification: profile.qualification,
          contactNumber: profile.contactNumber,
          email: profile.email,
          dateOfJoining: this.toDateInputValue(profile.dateOfJoining)
        });

        this.onDistrictChange();
      },
      error: (error) => {
        if (error?.status !== 404) {
          console.error('Error loading profile:', error);
        }
      }
    });
  }

  private toDateInputValue(value: any): string {
    const d = new Date(value);
    if (isNaN(d.getTime())) return '';
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  loadDistricts() {
    this.teacherService.getBiharDistricts().subscribe({
      next: (districts: District[]) => {
        this.districts = districts;
      },
      error: (error: any) => {
        console.error('Error loading districts:', error);
      }
    });
  }

  loadSchools() {
    this.schoolService.getAllSchools().subscribe({
      next: (schools: School[]) => {
        this.schools = schools;
      },
      error: (error: any) => {
        console.error('Error loading schools:', error);
      }
    });
  }

  onDistrictChange() {
    const selectedDistrict = this.profileForm.get('district')?.value;
    if (selectedDistrict) {
      const district = this.districts.find(d => d.name === selectedDistrict);
      this.pincodes = district?.pincodes || [];
    }
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const profileData = {
        ...this.profileForm.value,
        schoolId: parseInt(this.profileForm.value.schoolId)
      };

      const request$ = this.profileId
        ? this.userProfileService.updateProfile(this.profileId, { id: this.profileId, ...profileData })
        : this.userProfileService.createProfile(profileData);

      request$.subscribe({
        next: (response) => {
          this.isLoading = false;
          this.successMessage = this.profileId ? 'Profile updated successfully! Redirecting to dashboard...' : 'Profile created successfully! Redirecting to dashboard...';
          setTimeout(() => {
            this.router.navigate(['/user-dashboard']);
          }, 2000);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Failed to save profile. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched(this.profileForm);
    }
  }

  markFormGroupTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}
