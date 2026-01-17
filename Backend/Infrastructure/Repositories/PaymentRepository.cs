using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Include(p => p.ApprovedByUser)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetByOrderIdAsync(string orderId)
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
    {
        return await _context.Payments
            .Include(p => p.Subscription)
            .Where(p => p.UserId == userId && p.IsActive)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Where(p => p.Status == PaymentStatus.PendingApproval && p.IsActive)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsWithUserDetailsAsync()
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Where(p => p.Status == PaymentStatus.PendingApproval && p.IsActive)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        payment.CreatedDate = DateTime.UtcNow;
        payment.UpdatedDate = DateTime.UtcNow;
        
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        payment.UpdatedDate = DateTime.UtcNow;
        
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await GetByIdAsync(id);
        if (payment != null)
        {
            payment.IsActive = false;
            payment.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}