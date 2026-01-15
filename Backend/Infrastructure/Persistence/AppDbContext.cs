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

        base.OnModelCreating(modelBuilder);
    }
}