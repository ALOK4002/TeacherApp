import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-about-us',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="about-us-container ms-fadeIn">
      <div class="education-header ms-Card--elevated">
        <div class="ms-Flex ms-Flex--spaceBetween ms-Flex--wrap">
          <div>
            <h1 class="ms-fontSize-32 ms-fontWeight-bold">
              <span class="education-icon">🎓</span>
              Bihar Teacher Management Portal
            </h1>
            <p class="ms-fontSize-16" style="margin-top: 8px; opacity: 0.9;">
              Empowering Education Through Digital Innovation
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
            <button (click)="logout()" class="ms-Button ms-Button--danger">
              <span class="education-icon">🚪</span> Logout
            </button>
          </div>
        </div>
      </div>

      <div class="content-wrapper ms-Grid" style="grid-template-columns: 2fr 1fr; gap: 24px;">
        <!-- Left Section: Main Content -->
        <div class="main-content">
          <!-- Teacher Contribution Section -->
          <section class="ms-Card ms-Card--elevated ms-slideUp">
            <div class="section-header">
              <h2 class="ms-fontSize-28 ms-fontWeight-semibold" style="color: var(--primary-color); margin-bottom: 16px;">
                <span class="education-icon">🌟</span>
                The Noble Contribution of Teachers
              </h2>
              <div class="section-divider"></div>
            </div>
            
            <div class="contribution-content">
              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  Teachers are the architects of our society, the silent heroes who shape the minds and hearts of future generations. 
                  In Bihar, a state rich in cultural heritage and educational legacy, teachers have been the cornerstone of progress 
                  and enlightenment for centuries. From the ancient seats of learning in Nalanda and Vikramshila to the modern 
                  classrooms across our villages and cities, teachers have consistently illuminated the path of knowledge.
                </p>
              </div>
              
              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  The contribution of teachers extends far beyond the confines of textbooks and curricula. They are mentors, 
                  guides, and often the first source of inspiration for countless students. In rural Bihar, where challenges 
                  are manifold, teachers serve as beacons of hope, breaking barriers of poverty, illiteracy, and social 
                  inequality. They work tirelessly, often with limited resources, to ensure that every child receives the 
                  gift of education.
                </p>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  Teachers in Bihar have played pivotal roles in preserving our rich cultural traditions while embracing 
                  modern educational methodologies. They have been instrumental in promoting regional languages, literature, 
                  and arts, ensuring that our cultural identity remains intact while preparing students for a globalized world. 
                  Their dedication has produced scholars, scientists, administrators, and leaders who have made significant 
                  contributions to society.
                </p>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  The COVID-19 pandemic highlighted the irreplaceable role of teachers as they adapted to digital platforms, 
                  ensuring continuity of education despite unprecedented challenges. Many teachers in Bihar went beyond their 
                  call of duty, visiting students' homes, providing learning materials, and offering emotional support during 
                  difficult times. Their resilience and commitment exemplify the true spirit of teaching.
                </p>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  Teachers are not just educators; they are nation builders. They instill values of honesty, integrity, 
                  compassion, and social responsibility in their students. Through their guidance, students learn to think 
                  critically, solve problems creatively, and contribute meaningfully to society. The impact of a good teacher 
                  resonates through generations, creating a ripple effect of positive change.
                </p>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  In Bihar's journey towards educational excellence, teachers have been the driving force behind numerous 
                  success stories. From improving literacy rates to promoting inclusive education, from encouraging girls' 
                  education to supporting students with special needs, teachers have consistently championed the cause of 
                  equitable and quality education for all.
                </p>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  We salute the unwavering dedication of Bihar's teachers who continue to light the lamp of knowledge, 
                  nurturing young minds with patience, love, and wisdom. Their contribution to society is immeasurable, 
                  and their legacy will continue to inspire future generations. This platform is our humble tribute to 
                  their selfless service and our commitment to supporting them in their noble mission.
                </p>
              </div>
            </div>
          </section>

          <!-- Website Objective Section -->
          <section class="ms-Card ms-Card--elevated ms-slideUp" style="margin-top: 24px;">
            <div class="section-header">
              <h2 class="ms-fontSize-28 ms-fontWeight-semibold" style="color: var(--accent-education); margin-bottom: 16px;">
                <span class="education-icon">🎯</span>
                Our Mission: Bringing Teachers Together
              </h2>
              <div class="section-divider"></div>
            </div>
            
            <div class="objective-content">
              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  The Bihar Teacher Management Portal is designed with a singular vision: to create a unified platform 
                  that brings together the teaching community of Bihar, fostering collaboration, communication, and 
                  collective growth. In an era where technology has transformed every aspect of our lives, education 
                  and educators deserve a dedicated space that addresses their unique needs and challenges.
                </p>
              </div>

              <div class="objectives-grid ms-Grid" style="grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 16px; margin: 24px 0;">
                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-education);">🤝</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Community Building</h3>
                  <p class="ms-fontSize-14">Creating a vibrant community where teachers can connect, share experiences, and support each other 
                  in their educational journey.</p>
                </div>

                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-knowledge);">📢</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Communication Hub</h3>
                  <p class="ms-fontSize-14">Providing a centralized notice board system for important announcements, educational updates, 
                  and collaborative discussions.</p>
                </div>

                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-growth);">📊</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Resource Management</h3>
                  <p class="ms-fontSize-14">Streamlining teacher and school information management to improve administrative efficiency 
                  and resource allocation.</p>
                </div>

                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-creativity);">🎯</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Professional Development</h3>
                  <p class="ms-fontSize-14">Facilitating knowledge sharing, best practices exchange, and continuous professional development 
                  opportunities for educators.</p>
                </div>

                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-education);">🌐</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Digital Empowerment</h3>
                  <p class="ms-fontSize-14">Empowering teachers with digital tools and platforms to enhance their teaching effectiveness 
                  and reach.</p>
                </div>

                <div class="objective-card ms-Card">
                  <div class="card-icon" style="background: var(--gradient-knowledge);">📈</div>
                  <h3 class="ms-fontSize-18 ms-fontWeight-semibold">Educational Excellence</h3>
                  <p class="ms-fontSize-14">Supporting Bihar's vision of achieving educational excellence through collaborative efforts 
                  and innovative approaches.</p>
                </div>
              </div>

              <div class="content-paragraph">
                <p class="ms-fontSize-16">
                  This platform serves as a bridge between traditional teaching values and modern technological solutions. 
                  By bringing teachers together on a single platform, we aim to create synergies that will benefit not 
                  only the teaching community but also the students and the broader society. Together, we can build a 
                  stronger, more connected, and more effective educational ecosystem in Bihar.
                </p>
              </div>
            </div>
          </section>
        </div>

        <!-- Right Section: Sidebar -->
        <div class="sidebar">
          <!-- Top Schools of Patna -->
          <section class="ms-Card ms-Card--elevated ms-slideUp">
            <div class="section-header">
              <h3 class="ms-fontSize-20 ms-fontWeight-semibold" style="color: var(--accent-growth); margin-bottom: 16px;">
                <span class="education-icon">🏫</span>
                Top Schools of Patna
              </h3>
              <div class="section-divider"></div>
            </div>
            
            <div class="schools-list">
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">Delhi Public School, Patna</h4>
                  <span class="growth-indicator">Premier</span>
                </div>
                <p class="ms-fontSize-14">Premier CBSE affiliated school known for academic excellence and holistic development.</p>
              </div>
              
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">St. Xavier's High School</h4>
                  <span class="knowledge-badge">Historic</span>
                </div>
                <p class="ms-fontSize-14">Historic institution with over 150 years of educational legacy in Patna.</p>
              </div>
              
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">Notre Dame Academy</h4>
                  <span class="creativity-highlight">Excellence</span>
                </div>
                <p class="ms-fontSize-14">Leading girls' school focusing on women's empowerment and quality education.</p>
              </div>
              
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">Loyola High School</h4>
                  <span class="growth-indicator">Jesuit</span>
                </div>
                <p class="ms-fontSize-14">Jesuit institution known for character building and academic rigor.</p>
              </div>
              
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">St. Michael's High School</h4>
                  <span class="knowledge-badge">Renowned</span>
                </div>
                <p class="ms-fontSize-14">Renowned for its disciplined environment and excellent academic results.</p>
              </div>
              
              <div class="school-item ms-Card">
                <div class="school-header">
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">Patna Central School</h4>
                  <span class="creativity-highlight">Government</span>
                </div>
                <p class="ms-fontSize-14">Government school setting benchmarks in public education excellence.</p>
              </div>
            </div>
          </section>

          <!-- Chief Minister's Message -->
          <section class="ms-Card ms-Card--elevated ms-slideUp" style="margin-top: 24px;">
            <div class="section-header">
              <h3 class="ms-fontSize-20 ms-fontWeight-semibold" style="color: var(--accent-wisdom); margin-bottom: 16px;">
                <span class="education-icon">👨‍💼</span>
                Message from Hon'ble Chief Minister
              </h3>
              <div class="section-divider"></div>
            </div>
            
            <div class="cm-message">
              <div class="cm-photo ms-Card">
                <div class="photo-placeholder">
                  <div class="avatar-icon">📸</div>
                  <h4 class="ms-fontSize-16 ms-fontWeight-semibold">Shri Nitish Kumar</h4>
                  <p class="ms-fontSize-12">Hon'ble Chief Minister of Bihar</p>
                </div>
              </div>
              
              <div class="message-content ms-Card" style="background: var(--gradient-education); color: white; margin-top: 16px;">
                <blockquote class="ms-fontSize-14" style="line-height: 1.6;">
                  "शिक्षक हमारे समाज के आधार स्तंभ हैं। बिहार की शिक्षा व्यवस्था को मजबूत बनाने में शिक्षकों की भूमिका अत्यंत महत्वपूर्ण है। 
                  यह डिजिटल प्लेटफॉर्म हमारे शिक्षकों को एक साथ लाने और उनके बीच सहयोग बढ़ाने का एक सराहनीय प्रयास है।
                  
                  <br><br>
                  
                  Teachers are the foundation pillars of our society. The role of teachers is extremely important in 
                  strengthening Bihar's education system. This digital platform is a commendable effort to bring our 
                  teachers together and enhance cooperation among them.
                  
                  <br><br>
                  
                  I encourage all teachers of Bihar to actively participate in this initiative and contribute to building 
                  a stronger educational ecosystem. Together, we will make Bihar a leading state in education and ensure 
                  that every child receives quality education.
                  
                  <br><br>
                  
                  My best wishes to all the dedicated teachers of Bihar for their continued service to the nation."
                </blockquote>
                
                <div class="signature" style="margin-top: 16px; text-align: right; border-top: 1px solid rgba(255, 255, 255, 0.3); padding-top: 12px;">
                  <strong class="ms-fontSize-14">- Shri Nitish Kumar</strong><br>
                  <em class="ms-fontSize-12" style="opacity: 0.9;">Hon'ble Chief Minister, Bihar</em>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .about-us-container {
      padding: var(--spacing-xxl);
      max-width: 1400px;
      margin: 0 auto;
      min-height: 100vh;
    }

    .content-wrapper {
      margin-top: var(--spacing-xxl);
    }

    .section-header {
      margin-bottom: var(--spacing-l);
    }

    .section-divider {
      height: 3px;
      background: var(--gradient-education);
      border-radius: var(--border-radius-small);
      margin-top: var(--spacing-s);
    }

    .content-paragraph {
      margin-bottom: var(--spacing-l);
    }

    .content-paragraph p {
      color: var(--neutral-gray-120);
      line-height: 1.7;
      text-align: justify;
    }

    .objective-card {
      padding: var(--spacing-l);
      text-align: center;
      transition: all 0.2s ease-in-out;
      border-left: 4px solid transparent;
    }

    .objective-card:hover {
      transform: translateY(-4px);
      box-shadow: var(--shadow-16);
      border-left-color: var(--primary-color);
    }

    .card-icon {
      width: 48px;
      height: 48px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 auto var(--spacing-m);
      font-size: var(--font-size-20);
      color: white;
    }

    .objective-card h3 {
      color: var(--neutral-gray-120);
      margin-bottom: var(--spacing-s);
    }

    .objective-card p {
      color: var(--neutral-gray-90);
      line-height: 1.6;
      margin: 0;
    }

    .schools-list {
      display: flex;
      flex-direction: column;
      gap: var(--spacing-m);
    }

    .school-item {
      padding: var(--spacing-l);
      transition: all 0.2s ease-in-out;
      border-left: 4px solid var(--accent-growth);
    }

    .school-item:hover {
      transform: translateX(4px);
      box-shadow: var(--shadow-8);
    }

    .school-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: var(--spacing-s);
      flex-wrap: wrap;
      gap: var(--spacing-s);
    }

    .school-header h4 {
      color: var(--neutral-gray-120);
      margin: 0;
      flex: 1;
    }

    .school-item p {
      color: var(--neutral-gray-90);
      line-height: 1.6;
      margin: 0;
    }

    .cm-message {
      display: flex;
      flex-direction: column;
    }

    .photo-placeholder {
      text-align: center;
      padding: var(--spacing-l);
      background: var(--neutral-gray-20);
      border-radius: var(--border-radius-large);
    }

    .avatar-icon {
      font-size: var(--font-size-48);
      margin-bottom: var(--spacing-s);
      display: block;
    }

    .photo-placeholder h4 {
      color: var(--neutral-gray-120);
      margin: var(--spacing-s) 0 var(--spacing-xs);
    }

    .photo-placeholder p {
      color: var(--neutral-gray-90);
      margin: 0;
    }

    .message-content {
      padding: var(--spacing-l);
    }

    .message-content blockquote {
      margin: 0;
      padding: 0;
      font-style: italic;
      line-height: 1.7;
    }

    .signature {
      margin-top: var(--spacing-l);
      text-align: right;
      border-top: 1px solid rgba(255, 255, 255, 0.3);
      padding-top: var(--spacing-m);
    }

    /* Responsive Design */
    @media (max-width: 1024px) {
      .content-wrapper {
        grid-template-columns: 1fr !important;
      }
      
      .sidebar {
        order: -1;
      }
    }

    @media (max-width: 768px) {
      .about-us-container {
        padding: var(--spacing-l);
      }
      
      .education-header .ms-Flex {
        flex-direction: column;
        align-items: flex-start;
        gap: var(--spacing-l);
      }
      
      .education-header h1 {
        font-size: var(--font-size-24) !important;
      }
      
      .objectives-grid {
        grid-template-columns: 1fr !important;
      }
      
      .school-header {
        flex-direction: column;
        align-items: flex-start;
      }
    }

    @media (max-width: 480px) {
      .education-header {
        padding: var(--spacing-l);
      }
      
      .education-header h1 {
        font-size: var(--font-size-20) !important;
      }
      
      .ms-Card {
        padding: var(--spacing-m);
      }
    }
  `]
})
export class AboutUsComponent implements OnInit {
  userName = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.userName = this.authService.getUserName() || 'User';
  }

  goToNoticeBoard() {
    this.router.navigate(['/notice-board']);
  }

  goToTeachers() {
    this.router.navigate(['/teachers']);
  }

  goToSchools() {
    this.router.navigate(['/schools']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}