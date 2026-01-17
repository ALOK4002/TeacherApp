using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Notice> Notices { get; set; }
    public DbSet<NoticeReply> NoticeReplies { get; set; }
    public DbSet<TeacherDocument> TeacherDocuments { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollQuestion> PollQuestions { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<PollResponse> PollResponses { get; set; }
    public DbSet<PollAnswer> PollAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsApproved).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.UserName).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // School entity configuration
        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SchoolName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SchoolCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.District).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Block).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Village).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SchoolType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ManagementType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PrincipalName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.EstablishedDate).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.SchoolCode).IsUnique();
            entity.HasIndex(e => e.District);
            entity.HasIndex(e => e.Block);
        });

        // Teacher entity configuration
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TeacherName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.District).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Pincode).IsRequired().HasMaxLength(6);
            entity.Property(e => e.ClassTeaching).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Qualification).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DateOfJoining).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.ContactNumber);
            entity.HasIndex(e => e.District);
            
            // Foreign key relationship
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Notice entity configuration
        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Priority).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PostedByUserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PostedDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.PostedDate);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Priority);
            
            // Foreign key relationship
            entity.HasOne(e => e.PostedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.PostedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // NoticeReply entity configuration
        modelBuilder.Entity<NoticeReply>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ReplyMessage).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.RepliedByUserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RepliedDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.RepliedDate);
            
            // Foreign key relationships
            entity.HasOne(e => e.Notice)
                  .WithMany(n => n.Replies)
                  .HasForeignKey(e => e.NoticeId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.RepliedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.RepliedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // UserProfile entity configuration
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TeacherName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.District).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Pincode).IsRequired().HasMaxLength(6);
            entity.Property(e => e.ClassTeaching).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Qualification).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DateOfJoining).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.District);
            
            // Foreign key relationships
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // TeacherDocument entity configuration
        modelBuilder.Entity<TeacherDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomDocumentType).HasMaxLength(100);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.BlobUrl).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.BlobContainerName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BlobFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FileSizeInBytes).IsRequired();
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.UploadedDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.DocumentType);
            entity.HasIndex(e => e.UploadedDate);
            entity.HasIndex(e => e.UserId);
            
            // Foreign key relationships (both nullable for flexibility)
            entity.HasOne(e => e.Teacher)
                  .WithMany(t => t.Documents)
                  .HasForeignKey(e => e.TeacherId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired(false);
                  
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired(false);
        });

        // Subscription entity configuration
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Tier).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.DocumentUploadLimit).IsRequired();
            entity.Property(e => e.FileSizeLimitInBytes).IsRequired();
            entity.Property(e => e.DocumentsUploaded).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasIndex(e => e.Tier);
            entity.HasIndex(e => e.Status);
            
            // Foreign key relationship
            entity.HasOne(e => e.User)
                  .WithOne(u => u.Subscription)
                  .HasForeignKey<Subscription>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment entity configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Gateway).IsRequired();
            entity.Property(e => e.GatewayOrderId).HasMaxLength(100);
            entity.Property(e => e.GatewayTransactionId).HasMaxLength(100);
            entity.Property(e => e.GatewayResponse).HasMaxLength(2000);
            entity.Property(e => e.ChecksumHash).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.OrderId).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedDate);
            
            // Foreign key relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Payments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Subscription)
                  .WithMany(s => s.Payments)
                  .HasForeignKey(e => e.SubscriptionId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.ApprovedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        // UserActivity entity configuration
        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActivityType).IsRequired();
            entity.Property(e => e.ActivityDescription).IsRequired().HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.Metadata).HasMaxLength(2000);
            entity.Property(e => e.ActivityDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ActivityType);
            entity.HasIndex(e => e.ActivityDate);
            
            // Foreign key relationship
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Activities)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Poll entity configuration
        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.AllowMultipleVotes).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.CreatedDate);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.Type);
            
            // Foreign key relationship
            entity.HasOne(e => e.CreatedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // PollQuestion entity configuration
        modelBuilder.Entity<PollQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuestionText).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Order).IsRequired();
            entity.Property(e => e.IsRequired).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.PollId);
            entity.HasIndex(e => e.Order);
            
            // Foreign key relationship
            entity.HasOne(e => e.Poll)
                  .WithMany(p => p.Questions)
                  .HasForeignKey(e => e.PollId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PollOption entity configuration
        modelBuilder.Entity<PollOption>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OptionText).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Order).IsRequired();
            entity.Property(e => e.VoteCount).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.UpdatedDate).IsRequired();
            
            entity.HasIndex(e => e.PollQuestionId);
            entity.HasIndex(e => e.Order);
            
            // Foreign key relationship
            entity.HasOne(e => e.PollQuestion)
                  .WithMany(q => q.Options)
                  .HasForeignKey(e => e.PollQuestionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PollResponse entity configuration
        modelBuilder.Entity<PollResponse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RespondedDate).IsRequired();
            entity.Property(e => e.UserIpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();
            
            entity.HasIndex(e => e.PollId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RespondedDate);
            
            // Foreign key relationships
            entity.HasOne(e => e.Poll)
                  .WithMany(p => p.Responses)
                  .HasForeignKey(e => e.PollId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        // PollAnswer entity configuration
        modelBuilder.Entity<PollAnswer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TextAnswer).HasMaxLength(1000);
            entity.Property(e => e.RatingValue);
            entity.Property(e => e.AnsweredDate).IsRequired();
            
            entity.HasIndex(e => e.PollResponseId);
            entity.HasIndex(e => e.PollQuestionId);
            entity.HasIndex(e => e.PollOptionId);
            
            // Foreign key relationships
            entity.HasOne(e => e.PollResponse)
                  .WithMany(r => r.Answers)
                  .HasForeignKey(e => e.PollResponseId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.PollQuestion)
                  .WithMany(q => q.Answers)
                  .HasForeignKey(e => e.PollQuestionId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.PollOption)
                  .WithMany(o => o.Answers)
                  .HasForeignKey(e => e.PollOptionId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}