using Application.DTOs;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentOrderResponseDto> CreatePaymentOrderAsync(int userId, CreatePaymentOrderDto dto);
    Task<PaymentDto> HandlePaymentCallbackAsync(PaymentCallbackDto dto);
    Task<PaymentDto> ApprovePaymentAsync(int paymentId, int adminUserId);
    Task<PaymentDto> RejectPaymentAsync(int paymentId, int adminUserId, string rejectionReason);
    Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync();
    Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId);
}