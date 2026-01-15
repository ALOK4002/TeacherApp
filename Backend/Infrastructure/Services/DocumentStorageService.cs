using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class DocumentStorageService : IDocumentStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public DocumentStorageService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureStorage:ConnectionString"] 
            ?? throw new InvalidOperationException("Azure Storage connection string not configured");
        _containerName = configuration["AzureStorage:ContainerName"] ?? "student-documents";
    }

    public async Task<(string BlobUrl, string BlobFileName, string ContainerName)> UploadDocumentAsync(
        IFormFile file,
        int studentId,
        string documentType)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty or null");
        }

        // Create blob service client
        var blobServiceClient = new BlobServiceClient(_connectionString);
        
        // Get or create container
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        // Generate unique blob name
        var fileExtension = Path.GetExtension(file.FileName);
        var blobFileName = $"{studentId}/{documentType}/{Guid.NewGuid()}{fileExtension}";
        
        // Get blob client
        var blobClient = containerClient.GetBlobClient(blobFileName);

        // Set content type
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        // Upload file
        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });
        }

        return (blobClient.Uri.ToString(), blobFileName, _containerName);
    }

    public async Task<byte[]> DownloadDocumentAsync(string containerName, string blobFileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobFileName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob {blobFileName} not found");
        }

        using (var memoryStream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public async Task<bool> DeleteDocumentAsync(string containerName, string blobFileName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobFileName);

            return await blobClient.DeleteIfExistsAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetDocumentUrlAsync(string containerName, string blobFileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobFileName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob {blobFileName} not found");
        }

        return blobClient.Uri.ToString();
    }

    public async Task<bool> DocumentExistsAsync(string containerName, string blobFileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobFileName);

        return await blobClient.ExistsAsync();
    }
}
