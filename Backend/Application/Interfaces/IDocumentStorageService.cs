using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IDocumentStorageService
{
    Task<(string BlobUrl, string BlobFileName, string ContainerName)> UploadDocumentAsync(
        IFormFile file,
        int studentId,
        string documentType);
    
    Task<byte[]> DownloadDocumentAsync(string containerName, string blobFileName);
    
    Task<bool> DeleteDocumentAsync(string containerName, string blobFileName);
    
    Task<string> GetDocumentUrlAsync(string containerName, string blobFileName);
    
    Task<bool> DocumentExistsAsync(string containerName, string blobFileName);
}
