using Application.DTOs;

namespace Application.Interfaces;

public interface IPollService
{
    Task<IEnumerable<PollDto>> GetAllActivePollsAsync();
    Task<PollDto?> GetPollByIdAsync(int id);
    Task<PollDto> CreatePollAsync(CreatePollDto dto, int userId);
    Task<PollDto> UpdatePollAsync(UpdatePollDto dto);
    Task<bool> DeletePollAsync(int id, int userId);
    Task<IEnumerable<PollDto>> GetUserPollsAsync(int userId);
    Task<PollResultDto> GetPollResultsAsync(int pollId);
    Task<PollResponseDto> SubmitPollResponseAsync(SubmitPollResponseDto dto, int? userId, string? ipAddress, string? userAgent);
    Task<PollResponseDto?> GetUserPollResponseAsync(int pollId, int? userId, string? ipAddress);
}
