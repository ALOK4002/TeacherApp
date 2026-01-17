using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class TeacherDocumentService : ITeacherDocumentService
{
    private readonly ITeacherDocumentRepository _documentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IDocumentStorageService _storageService;
    private readonly IEmailService _emailService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUserActivityService _activityService;

    public TeacherDocumentService(
        ITeacherDocumentRepository documentRepository,
        ITeacherRepository teacherRepository,
        IDocumentStorageService storageService,
        IEmailService emailService,
        ISubscriptionService subscriptionService,
        IUserActivityService activityService)
    {
        _documentRepository = documentRepository;
        _teacherRepository = teacherRepository;
        _storageService = storageService;
        _emailService = emailService;
        _subscriptionService = subscriptionService;
        _activityService = activityService;
    }

    public async Task<TeacherDocumentDto> UploadDocumentAsync(
        int teacherId,
        IFormFile file,
        string documentType,
        string customDocumentType,
        string remarks,
        int uploadedByUserId)
    {
        // Verify teacher exists
        var teacher = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found");
        }

        // Upload to Azure Blob Storage
        var (blobUrl, blobFileName, containerName) = await _storageService.UploadDocumentAsync(
            file,
            teacherId,
            documentType);

        // Create document record
        var document = new TeacherDocument
        {
            TeacherId = teacherId,
            DocumentType = documentType,
            CustomDocumentType = customDocumentType,
            FileName = file.FileName,
            OriginalFileName = file.FileName,
            BlobUrl = blobUrl,
            BlobContainerName = containerName,
            BlobFileName = blobFileName,
            ContentType = file.ContentType,
            FileSizeInBytes = file.Length,
            Remarks = remarks,
            UploadedDate = DateTime.UtcNow,
            UploadedByUserId = uploadedByUserId
        };

        var savedDocument = await _documentRepository.AddAsync(document);
        return MapToDto(savedDocument);
    }

    public async Task<IEnumerable<TeacherDocumentDto>> GetDocumentsByTeacherIdAsync(int teacherId)
    {
        var documents = await _documentRepository.GetByTeacherIdAsync(teacherId);
        return documents.Select(MapToDto);
    }

    public async Task<TeacherDocumentDto> UploadUserDocumentAsync(
        int userId,
        IFormFile file,
        string documentType,
        string customDocumentType,
        string remarks)
    {
        // Check subscription limits
        var canUpload = await _subscriptionService.CanUploadDocumentAsync(userId, file.Length);
        if (!canUpload)
        {
            var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);
            if (subscription != null)
            {
                if (file.Length > subscription.FileSizeLimitInBytes)
                {
                    throw new InvalidOperationException($"File size exceeds limit. Maximum allowed: {subscription.FileSizeLimitFormatted}");
                }
                if (subscription.DocumentsUploaded >= subscription.DocumentUploadLimit)
                {
                    throw new InvalidOperationException($"Upload limit reached. You have uploaded {subscription.DocumentsUploaded}/{subscription.DocumentUploadLimit} documents. Upgrade to Premium for more uploads.");
                }
            }
            throw new InvalidOperationException("Cannot upload document. Please check your subscription limits.");
        }

        // Upload to Azure Blob Storage (use userId as folder)
        var (blobUrl, blobFileName, containerName) = await _storageService.UploadDocumentAsync(
            file,
            userId,
            documentType);

        // Create document record with UserId
        var document = new TeacherDocument
        {
            UserId = userId,
            TeacherId = null, // User-uploaded documents don't have TeacherId
            DocumentType = documentType,
            CustomDocumentType = customDocumentType,
            FileName = file.FileName,
            OriginalFileName = file.FileName,
            BlobUrl = blobUrl,
            BlobContainerName = containerName,
            BlobFileName = blobFileName,
            ContentType = file.ContentType,
            FileSizeInBytes = file.Length,
            Remarks = remarks,
            UploadedDate = DateTime.UtcNow,
            UploadedByUserId = userId
        };

        var savedDocument = await _documentRepository.AddAsync(document);

        // Increment document count in subscription
        await _subscriptionService.IncrementDocumentCountAsync(userId);

        // Log activity
        await _activityService.LogDocumentUploadAsync(userId, savedDocument.Id, file.FileName);

        return MapToDto(savedDocument);
    }

    public async Task<IEnumerable<TeacherDocumentDto>> GetDocumentsByUserIdAsync(int userId)
    {
        var documents = await _documentRepository.GetByUserIdAsync(userId);
        return documents.Select(MapToDto);
    }

    public async Task<TeacherDocumentDto?> GetDocumentByIdAsync(int id)
    {
        var document = await _documentRepository.GetByIdAsync(id);
        return document != null ? MapToDto(document) : null;
    }

    public async Task<byte[]> DownloadDocumentAsync(int documentId)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
        {
            throw new FileNotFoundException("Document not found");
        }

        return await _storageService.DownloadDocumentAsync(
            document.BlobContainerName,
            document.BlobFileName);
    }

    public async Task<bool> DeleteDocumentAsync(int documentId, int userId)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
        {
            return false;
        }

        // Check if user owns this document
        if (document.UserId.HasValue && document.UserId.Value != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own documents");
        }

        // Delete from Azure Blob Storage
        await _storageService.DeleteDocumentAsync(
            document.BlobContainerName,
            document.BlobFileName);

        // Log activity before deletion
        await _activityService.LogDocumentDeleteAsync(userId, documentId, document.OriginalFileName);

        // Soft delete from database
        return await _documentRepository.DeleteAsync(documentId);
    }

    public async Task<PagedResult<TeacherDocumentDto>> SearchDocumentsAsync(DocumentSearchRequest request)
    {
        var (documents, totalCount) = await _documentRepository.SearchDocumentsAsync(
            request.TeacherId,
            request.DocumentType,
            request.SearchTerm,
            request.FromDate,
            request.ToDate,
            request.Page,
            request.PageSize);

        var documentDtos = documents.Select(MapToDto).ToList();

        return new PagedResult<TeacherDocumentDto>
        {
            Items = documentDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<bool> SendDocumentByEmailAsync(SendDocumentEmailDto dto)
    {
        var document = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (document == null)
        {
            throw new FileNotFoundException("Document not found");
        }

        // Download document from Azure
        var fileBytes = await _storageService.DownloadDocumentAsync(
            document.BlobContainerName,
            document.BlobFileName);

        // Prepare email
        var teacherName = document.Teacher?.TeacherName ?? "Teacher";
        var subject = $"Document from Bihar Teacher Portal - {teacherName}";
        var body = $@"Dear {dto.RecipientName},

{dto.Message}

Please find the attached document: {document.OriginalFileName}

Document Details:
- Type: {document.DocumentType} {(!string.IsNullOrEmpty(document.CustomDocumentType) ? $"({document.CustomDocumentType})" : "")}
- Teacher: {teacherName}
- Uploaded: {document.UploadedDate:yyyy-MM-dd}
- Remarks: {document.Remarks}

Best regards,
Bihar Teacher Portal Team";

        // Send email with attachment
        return await _emailService.SendEmailAsync(
            dto.RecipientEmail,
            dto.RecipientName,
            subject,
            body,
            fileBytes,
            document.OriginalFileName,
            document.ContentType);
    }

    private static TeacherDocumentDto MapToDto(TeacherDocument document)
    {
        return new TeacherDocumentDto
        {
            Id = document.Id,
            TeacherId = document.TeacherId ?? 0,
            TeacherName = document.Teacher?.TeacherName ?? "",
            DocumentType = document.DocumentType,
            CustomDocumentType = document.CustomDocumentType,
            FileName = document.FileName,
            OriginalFileName = document.OriginalFileName,
            BlobUrl = document.BlobUrl,
            ContentType = document.ContentType,
            FileSizeInBytes = document.FileSizeInBytes,
            FileSizeFormatted = FormatFileSize(document.FileSizeInBytes),
            Remarks = document.Remarks,
            UploadedDate = document.UploadedDate,
            UploadedByUserId = document.UploadedByUserId,
            IsActive = document.IsActive
        };
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
