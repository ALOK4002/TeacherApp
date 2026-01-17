using Domain.Entities;

namespace Domain.Interfaces;

public interface IPollRepository
{
    Task<IEnumerable<Poll>> GetAllActiveAsync();
    Task<Poll?> GetByIdAsync(int id);
    Task<Poll> AddAsync(Poll poll);
    Task<Poll> UpdateAsync(Poll poll);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Poll>> GetByUserIdAsync(int userId);
    Task<bool> HasUserRespondedAsync(int pollId, int? userId, string? ipAddress);
}
