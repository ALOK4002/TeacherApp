using Domain.Entities;

namespace Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment?> GetByOrderIdAsync(string orderId);
    Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
    Task<IEnumerable<Payment>> GetPendingPaymentsWithUserDetailsAsync();
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
}