using Application.Interfaces;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _connectionString;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureEmail:ConnectionString"] 
            ?? throw new InvalidOperationException("Azure Email connection string not configured");
        _senderEmail = configuration["AzureEmail:SenderEmail"] 
            ?? "DoNotReply@biharteacherportal.com";
        _senderName = configuration["AzureEmail:SenderName"] 
            ?? "Bihar Teacher Portal";
    }

    public async Task<bool> SendEmailAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        byte[]? attachment = null,
        string? attachmentFileName = null,
        string? attachmentContentType = null)
    {
        try
        {
            var emailClient = new EmailClient(_connectionString);

            var emailContent = new EmailContent(subject)
            {
                PlainText = body,
                Html = $"<html><body><pre>{body}</pre></body></html>"
            };

            var emailMessage = new EmailMessage(
                senderAddress: _senderEmail,
                content: emailContent,
                recipients: new EmailRecipients(new List<EmailAddress> 
                { 
                    new EmailAddress(toEmail, toName) 
                }));

            if (attachment != null && !string.IsNullOrEmpty(attachmentFileName))
            {
                var attachmentContent = new BinaryData(attachment);
                var emailAttachment = new EmailAttachment(
                    attachmentFileName,
                    attachmentContentType ?? "application/octet-stream",
                    attachmentContent);
                
                emailMessage.Attachments.Add(emailAttachment);
            }

            EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage);

            return emailSendOperation.HasCompleted && !emailSendOperation.HasValue;
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Email send failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendEmailWithMultipleAttachmentsAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        List<(byte[] Content, string FileName, string ContentType)>? attachments = null)
    {
        try
        {
            var emailClient = new EmailClient(_connectionString);

            var emailContent = new EmailContent(subject)
            {
                PlainText = body,
                Html = $"<html><body><pre>{body}</pre></body></html>"
            };

            var emailMessage = new EmailMessage(
                senderAddress: _senderEmail,
                content: emailContent,
                recipients: new EmailRecipients(new List<EmailAddress> 
                { 
                    new EmailAddress(toEmail, toName) 
                }));

            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    var attachmentContent = new BinaryData(attachment.Content);
                    var emailAttachment = new EmailAttachment(
                        attachment.FileName,
                        attachment.ContentType,
                        attachmentContent);
                    
                    emailMessage.Attachments.Add(emailAttachment);
                }
            }

            EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage);

            return emailSendOperation.HasCompleted;
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Email send failed: {ex.Message}");
            return false;
        }
    }
}
