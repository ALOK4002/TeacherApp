namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        byte[]? attachment = null,
        string? attachmentFileName = null,
        string? attachmentContentType = null);
    
    Task<bool> SendEmailWithMultipleAttachmentsAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        List<(byte[] Content, string FileName, string ContentType)>? attachments = null);
}
