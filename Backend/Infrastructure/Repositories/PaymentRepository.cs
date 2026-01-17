using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(AppDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var query = _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Include(p => p.ApprovedByUser)
            .Where(p => p.Id == id);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<Payment?> GetByOrderIdAsync(string orderId)
    {
        _logger.LogInformation("Entering GetByOrderIdAsync for OrderId: {OrderId}", orderId);
        var query = _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Where(p => p.OrderId == orderId);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByOrderIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation("Entering GetByUserIdAsync for UserId: {UserId}", userId);
        var query = _context.Payments
            .Include(p => p.Subscription)
            .Where(p => p.UserId == userId && p.IsActive)
            .OrderByDescending(p => p.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetByUserIdAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
    {
        _logger.LogInformation("Entering GetPendingPaymentsAsync");
        var query = _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Where(p => p.Status == PaymentStatus.PendingApproval && p.IsActive)
            .OrderByDescending(p => p.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetPendingPaymentsAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsWithUserDetailsAsync()
    {
        _logger.LogInformation("Entering GetPendingPaymentsWithUserDetailsAsync");
        var query = _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
            .Where(p => p.Status == PaymentStatus.PendingApproval && p.IsActive)
            .OrderByDescending(p => p.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetPendingPaymentsWithUserDetailsAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _logger.LogInformation("Entering AddAsync for OrderId: {OrderId}", payment.OrderId);
        payment.CreatedDate = DateTime.UtcNow;
        payment.UpdatedDate = DateTime.UtcNow;
        
        _context.Payments.Add(payment);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", payment.Id);
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", payment.Id);
        payment.UpdatedDate = DateTime.UtcNow;
        
        _context.Payments.Update(payment);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", payment.Id);
        return payment;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Entering DeleteAsync for Id: {Id}", id);
        var payment = await GetByIdAsync(id);
        if (payment != null)
        {
            payment.IsActive = false;
            payment.UpdatedDate = DateTime.UtcNow;
            _logger.LogInformation("SQL SaveChanges for DeleteAsync (soft delete) Id: {Id}", id);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Exiting DeleteAsync successfully for Id: {Id}", id);
        }
        else
        {
            _logger.LogWarning("DeleteAsync: Payment not found for Id: {Id}", id);
        }
    }
}