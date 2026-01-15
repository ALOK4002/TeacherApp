using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherDocumentController : ControllerBase
{
    private readonly ITeacherDocumentService _documentService;
    private readonly ILogger<TeacherDocumentController> _logger;

    public TeacherDocumentController(
        ITeacherDocumentService documentService,
        ILogger<TeacherDocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument(
        [FromForm] int teacherId,
        [FromForm] IFormFile file,
        [FromForm] string documentType,
        [FromForm] string? customDocumentType,
        [FromForm] string? remarks)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            // Get user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            var document = await _documentService.UploadDocumentAsync(
                teacherId,
                file,
                documentType,
                customDocumentType ?? string.Empty,
                remarks ?? string.Empty,
                userId);

            return Ok(document);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document");
            return StatusCode(500, new { message = "An error occurred while uploading the document" });
        }
    }

    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetDocumentsByTeacher(int teacherId)
    {
        try
        {
            var documents = await _documentService.GetDocumentsByTeacherIdAsync(teacherId);
            return Ok(documents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving documents");
            return StatusCode(500, new { message = "An error occurred while retrieving documents" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(int id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound(new { message = "Document not found" });
            }
            return Ok(document);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving document");
            return StatusCode(500, new { message = "An error occurred while retrieving the document" });
        }
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound(new { message = "Document not found" });
            }

            var fileBytes = await _documentService.DownloadDocumentAsync(id);
            return File(fileBytes, document.ContentType, document.OriginalFileName);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading document");
            return StatusCode(500, new { message = "An error occurred while downloading the document" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _documentService.DeleteDocumentAsync(id, userId);
            if (!result)
            {
                return NotFound(new { message = "Document not found" });
            }
            return Ok(new { message = "Document deleted successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document");
            return StatusCode(500, new { message = "An error occurred while deleting the document" });
        }
    }

    // User document endpoints
    [HttpPost("upload-my-document")]
    public async Task<IActionResult> UploadMyDocument(
        [FromForm] IFormFile file,
        [FromForm] string documentType,
        [FromForm] string? customDocumentType,
        [FromForm] string? remarks)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var document = await _documentService.UploadUserDocumentAsync(
                userId,
                file,
                documentType,
                customDocumentType ?? string.Empty,
                remarks ?? string.Empty);

            return Ok(document);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading user document");
            return StatusCode(500, new { message = "An error occurred while uploading the document" });
        }
    }

    [HttpGet("my-documents")]
    public async Task<IActionResult> GetMyDocuments()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var documents = await _documentService.GetDocumentsByUserIdAsync(userId);
            return Ok(documents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user documents");
            return StatusCode(500, new { message = "An error occurred while retrieving documents" });
        }
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchDocuments([FromBody] DocumentSearchRequest request)
    {
        try
        {
            var result = await _documentService.SearchDocumentsAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching documents");
            return StatusCode(500, new { message = "An error occurred while searching documents" });
        }
    }

    [HttpPost("{id}/send-email")]
    public async Task<IActionResult> SendDocumentByEmail(int id, [FromBody] SendDocumentEmailDto dto)
    {
        try
        {
            dto.DocumentId = id;
            var result = await _documentService.SendDocumentByEmailAsync(dto);
            if (result)
            {
                return Ok(new { message = "Email sent successfully" });
            }
            return StatusCode(500, new { message = "Failed to send email" });
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return StatusCode(500, new { message = "An error occurred while sending the email" });
        }
    }
}
