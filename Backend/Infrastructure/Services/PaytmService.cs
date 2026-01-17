using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class PaytmService : IPaytmService
{
    private readonly string _merchantId;
    private readonly string _merchantKey;
    private readonly string _website;
    private readonly string _industryType;
    private readonly string _channelId;
    private readonly string _environment;
    private readonly string _callbackUrl;

    public PaytmService(IConfiguration configuration)
    {
        _merchantId = configuration["Paytm:MerchantId"] ?? throw new InvalidOperationException("Paytm MerchantId not configured");
        _merchantKey = configuration["Paytm:MerchantKey"] ?? throw new InvalidOperationException("Paytm MerchantKey not configured");
        _website = configuration["Paytm:Website"] ?? "DEFAULT";
        _industryType = configuration["Paytm:IndustryType"] ?? "Retail";
        _channelId = configuration["Paytm:ChannelId"] ?? "WEB";
        _environment = configuration["Paytm:Environment"] ?? "Staging";
        _callbackUrl = configuration["Paytm:CallbackUrl"] ?? throw new InvalidOperationException("Paytm CallbackUrl not configured");
    }

    public string GenerateChecksum(Dictionary<string, string> parameters)
    {
        var sortedParams = parameters.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        var paramString = string.Join("&", sortedParams.Select(x => $"{x.Key}={x.Value}"));
        
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_merchantKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(paramString));
        return Convert.ToBase64String(hash);
    }

    public bool VerifyChecksum(Dictionary<string, string> parameters, string checksum)
    {
        var generatedChecksum = GenerateChecksum(parameters);
        return generatedChecksum.Equals(checksum, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<Dictionary<string, string>> GetTransactionStatusAsync(string orderId)
    {
        var parameters = new Dictionary<string, string>
        {
            ["MID"] = _merchantId,
            ["ORDERID"] = orderId
        };

        var checksum = GenerateChecksum(parameters);
        parameters["CHECKSUMHASH"] = checksum;

        var url = GetPaytmStatusUrl();
        
        using var httpClient = new HttpClient();
        var content = new FormUrlEncodedContent(parameters);
        var response = await httpClient.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        // Parse Paytm response (usually in key=value format)
        var result = new Dictionary<string, string>();
        var pairs = responseString.Split('&');
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                result[keyValue[0]] = keyValue[1];
            }
        }

        return result;
    }

    public string GetPaytmUrl()
    {
        return _environment.ToLower() == "production" 
            ? "https://securegw.paytm.in/order/process"
            : "https://securegw-stage.paytm.in/order/process";
    }

    private string GetPaytmStatusUrl()
    {
        return _environment.ToLower() == "production"
            ? "https://securegw.paytm.in/order/status"
            : "https://securegw-stage.paytm.in/order/status";
    }
}