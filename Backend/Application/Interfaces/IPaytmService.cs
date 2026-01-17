namespace Application.Interfaces;

public interface IPaytmService
{
    string GenerateChecksum(Dictionary<string, string> parameters);
    bool VerifyChecksum(Dictionary<string, string> parameters, string checksum);
    Task<Dictionary<string, string>> GetTransactionStatusAsync(string orderId);
    string GetPaytmUrl();
}