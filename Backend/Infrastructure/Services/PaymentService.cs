using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaytmService _paytmService;
    private readonly IUserActivityService _activityService;
    private readonly IConfiguration _configuration;
    private readonly decimal _premiumAmount;

    public PaymentService(
        IPaymentRepository paymentRepository,
        ISubscriptionRepository subscriptionRepository,
        IPaytmService paytmService,
        IUserActivityService activityService,
        IConfiguration configuration)
    {
        _paymentRepository = paymentRepository;
        _subscriptionRepository = subscriptionRepository;
        _paytmService = paytmService;
        _activityService = activityService;
        _configuration = configuration;
        
        // Parse premium amount from configuration
        var premiumAmountString = configuration["Paytm:PremiumAmountInPaise"] ?? "9900"; // Default 99 rupees in paise
        if (decimal.TryParse(premiumAmountString, out var premiumAmountInPaise))
        {
            _premiumAmount = premiumAmountInPaise / 100m; // Convert paise to rupees
        }
        else
        {
            _premiumAmount = 99m; // Default fallback
        }
    }

    public async Task<PaymentOrderResponseDto> CreatePaymentOrderAsync(int userId, CreatePaymentOrderDto dto)
    {
        // Create subscription if not exists
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscription == null)
        {
            throw new InvalidOperationException("User subscription not found");
        }

        if (subscription.Tier == SubscriptionTier.Premium)
        {
            throw new InvalidOperationException("User already has premium subscription");
        }

        var orderId = $"ORDER_{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        var amount = _premiumAmount > 0 ? _premiumAmount : dto.Amount;

        // Create payment record
        var payment = new Payment
        {
            UserId = userId,
            SubscriptionId = subscription.Id,
            OrderId = orderId,
            Amount = amount,
            Currency = dto.Currency,
            Status = PaymentStatus.Pending,
            Gateway = PaymentGateway.Paytm,
            IsActive = true
        };

        var createdPayment = await _paymentRepository.AddAsync(payment);

        // Generate Paytm parameters
        var paytmParams = new Dictionary<string, string>
        {
            ["MID"] = GetMerchantId(),
            ["ORDER_ID"] = orderId,
            ["TXN_AMOUNT"] = amount.ToString("F2"),
            ["CUST_ID"] = userId.ToString(),
            ["INDUSTRY_TYPE_ID"] = "Retail",
            ["WEBSITE"] = "DEFAULT",
            ["CHANNEL_ID"] = "WEB",
            ["CALLBACK_URL"] = GetCallbackUrl()
        };

        var checksum = _paytmService.GenerateChecksum(paytmParams);

        // Log activity
        await _activityService.LogPaymentInitiatedAsync(userId, createdPayment.Id, amount);

        return new PaymentOrderResponseDto
        {
            OrderId = orderId,
            Amount = amount,
            Currency = dto.Currency,
            MerchantId = GetMerchantId(),
            ChecksumHash = checksum,
            CallbackUrl = GetCallbackUrl(),
            PaytmUrl = _paytmService.GetPaytmUrl()
        };
    }

    public async Task<PaymentDto> HandlePaymentCallbackAsync(PaymentCallbackDto dto)
    {
        var payment = await _paymentRepository.GetByOrderIdAsync(dto.OrderId);
        if (payment == null)
        {
            throw new InvalidOperationException("Payment not found");
        }

        // Verify checksum
        var paytmParams = new Dictionary<string, string>
        {
            ["ORDERID"] = dto.OrderId,
            ["TXNID"] = dto.TransactionId,
            ["TXNAMOUNT"] = dto.Amount,
            ["STATUS"] = dto.Status
        };

        if (!_paytmService.VerifyChecksum(paytmParams, dto.ChecksumHash))
        {
            payment.Status = PaymentStatus.Failed;
            payment.GatewayResponse = "Checksum verification failed";
            await _paymentRepository.UpdateAsync(payment);
            throw new InvalidOperationException("Payment verification failed");
        }

        // Update payment based on status
        payment.GatewayTransactionId = dto.TransactionId;
        payment.ChecksumHash = dto.ChecksumHash;
        payment.GatewayResponse = dto.GatewayResponse;

        if (dto.Status.ToUpper() == "TXN_SUCCESS")
        {
            payment.Status = PaymentStatus.PendingApproval; // Requires admin approval
            payment.PaymentDate = DateTime.UtcNow;
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
        }

        var updatedPayment = await _paymentRepository.UpdateAsync(payment);

        if (payment.Status == PaymentStatus.PendingApproval)
        {
            await _activityService.LogPaymentCompletedAsync(payment.UserId, payment.Id, payment.Amount);
        }

        return MapToDto(updatedPayment);
    }

    public async Task<PaymentDto> ApprovePaymentAsync(int paymentId, int adminUserId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new InvalidOperationException("Payment not found");
        }

        if (payment.Status != PaymentStatus.PendingApproval)
        {
            throw new InvalidOperationException("Payment is not pending approval");
        }

        payment.Status = PaymentStatus.Approved;
        payment.ApprovedByUserId = adminUserId;
        payment.ApprovedDate = DateTime.UtcNow;

        var updatedPayment = await _paymentRepository.UpdateAsync(payment);

        // Upgrade subscription to premium
        var subscription = await _subscriptionRepository.GetByIdAsync(payment.SubscriptionId);
        if (subscription != null)
        {
            var oldTier = subscription.Tier;
            subscription.Tier = SubscriptionTier.Premium;
            subscription.Status = SubscriptionStatus.Active;
            subscription.DocumentUploadLimit = 10;
            subscription.FileSizeLimitInBytes = 1048576; // 1MB
            subscription.EndDate = DateTime.UtcNow.AddYears(1);
            await _subscriptionRepository.UpdateAsync(subscription);

            await _activityService.LogSubscriptionUpgradeAsync(payment.UserId, oldTier, SubscriptionTier.Premium);
        }

        return MapToDto(updatedPayment);
    }

    public async Task<PaymentDto> RejectPaymentAsync(int paymentId, int adminUserId, string rejectionReason)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new InvalidOperationException("Payment not found");
        }

        if (payment.Status != PaymentStatus.PendingApproval)
        {
            throw new InvalidOperationException("Payment is not pending approval");
        }

        payment.Status = PaymentStatus.Rejected;
        payment.ApprovedByUserId = adminUserId;
        payment.ApprovedDate = DateTime.UtcNow;
        payment.RejectionReason = rejectionReason;

        var updatedPayment = await _paymentRepository.UpdateAsync(payment);
        return MapToDto(updatedPayment);
    }

    public async Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync()
    {
        var payments = await _paymentRepository.GetPendingPaymentsWithUserDetailsAsync();
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId)
    {
        var payments = await _paymentRepository.GetByUserIdAsync(userId);
        return payments.Select(MapToDto);
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            UserId = payment.UserId,
            OrderId = payment.OrderId,
            TransactionId = payment.TransactionId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Status = payment.Status,
            Gateway = payment.Gateway,
            PaymentDate = payment.PaymentDate,
            ApprovedDate = payment.ApprovedDate,
            RejectionReason = payment.RejectionReason,
            CreatedDate = payment.CreatedDate,
            UserName = payment.User?.UserName,
            UserEmail = payment.User?.Email,
            PaymentMethod = payment.Gateway.ToString(),
            ApprovedByUserId = payment.ApprovedByUserId
        };
    }

    private string GetMerchantId()
    {
        return _configuration["Paytm:MerchantId"] ?? throw new InvalidOperationException("Paytm MerchantId not configured");
    }

    private string GetCallbackUrl()
    {
        return _configuration["Paytm:CallbackUrl"] ?? throw new InvalidOperationException("Paytm CallbackUrl not configured");
    }
}