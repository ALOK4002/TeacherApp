using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface ITeacherDocumentService
{
    Task<TeacherDocumentDto> UploadDocumentAsync(
        int teacherId,
        IFormFile file,
        string documentType,
        string customDocumentType,
        string remarks,
        int uploadedByUserId);
    
    Task<TeacherDocumentDto> UploadUserDocumentAsync(
        int userId,
        IFormFile file,
        string documentType,
        string customDocumentType,
        string remarks);
    
    Task<IEnumerable<TeacherDocumentDto>> GetDocumentsByTeacherIdAsync(int teacherId);
    
    Task<IEnumerable<TeacherDocumentDto>> GetDocumentsByUserIdAsync(int userId);
    
    Task<TeacherDocumentDto?> GetDocumentByIdAsync(int id);
    
    Task<byte[]> DownloadDocumentAsync(int documentId);
    
    Task<bool> DeleteDocumentAsync(int documentId, int userId);
    
    Task<PagedResult<TeacherDocumentDto>> SearchDocumentsAsync(DocumentSearchRequest request);
    
    Task<bool> SendDocumentByEmailAsync(SendDocumentEmailDto dto);
}
