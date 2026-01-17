using Domain.Entities;

namespace Domain.Interfaces;

public interface IPollResponseRepository
{
    Task<PollResponse?> GetByIdAsync(int id);
    Task<PollResponse> AddAsync(PollResponse response);
    Task<PollResponse> UpdateAsync(PollResponse response);
    Task<IEnumerable<PollResponse>> GetByPollIdAsync(int pollId);
    Task<PollResponse?> GetUserResponseAsync(int pollId, int? userId, string? ipAddress);
}
