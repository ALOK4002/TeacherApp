using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PollService : IPollService
{
    private readonly IPollRepository _pollRepository;
    private readonly IPollResponseRepository _pollResponseRepository;
    private readonly ILogger<PollService> _logger;

    public PollService(
        IPollRepository pollRepository,
        IPollResponseRepository pollResponseRepository,
        ILogger<PollService> logger)
    {
        _pollRepository = pollRepository;
        _pollResponseRepository = pollResponseRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PollDto>> GetAllActivePollsAsync()
    {
        _logger.LogInformation("Entering GetAllActivePollsAsync");
        var polls = await _pollRepository.GetAllActiveAsync();
        var pollDtos = polls.Select(MapToPollDto);
        _logger.LogInformation("Exiting GetAllActivePollsAsync with count: {Count}", pollDtos.Count());
        return pollDtos;
    }

    public async Task<PollDto?> GetPollByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetPollByIdAsync with Id: {Id}", id);
        var poll = await _pollRepository.GetByIdAsync(id);
        var result = poll != null ? MapToPollDto(poll) : null;
        _logger.LogInformation("Exiting GetPollByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<PollDto> CreatePollAsync(CreatePollDto dto, int userId)
    {
        _logger.LogInformation("Entering CreatePollAsync for Title: {Title}", dto.Title);
        
        var poll = new Poll
        {
            Title = dto.Title,
            Description = dto.Description,
            Type = dto.Type,
            AllowMultipleVotes = dto.AllowMultipleVotes,
            EndDate = dto.EndDate,
            CreatedByUserId = userId,
            Questions = dto.Questions.Select((q, index) => new PollQuestion
            {
                QuestionText = q.QuestionText,
                Type = q.Type,
                Order = index,
                IsRequired = q.IsRequired,
                Options = q.Options?.Select((o, optIndex) => new PollOption
                {
                    OptionText = o.OptionText,
                    Order = optIndex
                }).ToList() ?? new List<PollOption>()
            }).ToList()
        };

        var createdPoll = await _pollRepository.AddAsync(poll);
        var result = MapToPollDto(createdPoll);
        _logger.LogInformation("Exiting CreatePollAsync with Id: {Id}", result.Id);
        return result;
    }

    public async Task<PollDto> UpdatePollAsync(UpdatePollDto dto)
    {
        _logger.LogInformation("Entering UpdatePollAsync for Id: {Id}", dto.Id);
        
        var existingPoll = await _pollRepository.GetByIdAsync(dto.Id);
        if (existingPoll == null)
        {
            throw new InvalidOperationException($"Poll with ID {dto.Id} not found");
        }

        existingPoll.Title = dto.Title;
        existingPoll.Description = dto.Description;
        existingPoll.Type = dto.Type;
        existingPoll.AllowMultipleVotes = dto.AllowMultipleVotes;
        existingPoll.EndDate = dto.EndDate;
        existingPoll.UpdatedDate = DateTime.UtcNow;

        var updatedPoll = await _pollRepository.UpdateAsync(existingPoll);
        var result = MapToPollDto(updatedPoll);
        _logger.LogInformation("Exiting UpdatePollAsync for Id: {Id}", result.Id);
        return result;
    }

    public async Task<bool> DeletePollAsync(int id, int userId)
    {
        _logger.LogInformation("Entering DeletePollAsync for Id: {Id}, UserId: {UserId}", id, userId);
        
        var poll = await _pollRepository.GetByIdAsync(id);
        if (poll == null || poll.CreatedByUserId != userId)
        {
            _logger.LogWarning("DeletePollAsync: Poll not found or access denied for Id: {Id}", id);
            return false;
        }

        var result = await _pollRepository.DeleteAsync(id);
        _logger.LogInformation("Exiting DeletePollAsync with result: {Result}", result);
        return result;
    }

    public async Task<IEnumerable<PollDto>> GetUserPollsAsync(int userId)
    {
        _logger.LogInformation("Entering GetUserPollsAsync for UserId: {UserId}", userId);
        var polls = await _pollRepository.GetByUserIdAsync(userId);
        var pollDtos = polls.Select(MapToPollDto);
        _logger.LogInformation("Exiting GetUserPollsAsync with count: {Count}", pollDtos.Count());
        return pollDtos;
    }

    public async Task<PollResultDto> GetPollResultsAsync(int pollId)
    {
        _logger.LogInformation("Entering GetPollResultsAsync for PollId: {PollId}", pollId);
        
        var poll = await _pollRepository.GetByIdAsync(pollId);
        if (poll == null)
        {
            throw new InvalidOperationException($"Poll with ID {pollId} not found");
        }

        var responses = await _pollResponseRepository.GetByPollIdAsync(pollId);
        var result = MapToPollResultDto(poll, responses);
        _logger.LogInformation("Exiting GetPollResultsAsync for PollId: {PollId}", pollId);
        return result;
    }

    public async Task<PollResponseDto> SubmitPollResponseAsync(SubmitPollResponseDto dto, int? userId, string? ipAddress, string? userAgent)
    {
        _logger.LogInformation("Entering SubmitPollResponseAsync for PollId: {PollId}, UserId: {UserId}", dto.PollId, userId);
        
        var poll = await _pollRepository.GetByIdAsync(dto.PollId);
        if (poll == null)
        {
            throw new InvalidOperationException($"Poll with ID {dto.PollId} not found");
        }

        if (poll.EndDate.HasValue && poll.EndDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Poll has ended");
        }

        var hasResponded = await _pollRepository.HasUserRespondedAsync(dto.PollId, userId, ipAddress);
        if (hasResponded && !poll.AllowMultipleVotes)
        {
            throw new InvalidOperationException("You have already responded to this poll");
        }

        var pollResponse = new PollResponse
        {
            PollId = dto.PollId,
            UserId = userId,
            UserIpAddress = ipAddress,
            UserAgent = userAgent,
            Answers = dto.Answers.Select(a => new PollAnswer
            {
                PollQuestionId = a.PollQuestionId,
                PollOptionId = a.PollOptionId,
                TextAnswer = a.TextAnswer,
                RatingValue = a.RatingValue
            }).ToList()
        };

        var createdResponse = await _pollResponseRepository.AddAsync(pollResponse);

        // Update vote counts for options
        foreach (var answer in createdResponse.Answers.Where(a => a.PollOptionId.HasValue))
        {
            var option = poll.Questions
                .SelectMany(q => q.Options)
                .FirstOrDefault(o => o.Id == answer.PollOptionId);
            if (option != null)
            {
                option.VoteCount++;
            }
        }

        await _pollRepository.UpdateAsync(poll);

        var result = MapToPollResponseDto(createdResponse);
        _logger.LogInformation("Exiting SubmitPollResponseAsync with ResponseId: {Id}", result.Id);
        return result;
    }

    public async Task<PollResponseDto?> GetUserPollResponseAsync(int pollId, int? userId, string? ipAddress)
    {
        _logger.LogInformation("Entering GetUserPollResponseAsync for PollId: {PollId}, UserId: {UserId}", pollId, userId);
        var response = await _pollResponseRepository.GetUserResponseAsync(pollId, userId, ipAddress);
        var result = response != null ? MapToPollResponseDto(response) : null;
        _logger.LogInformation("Exiting GetUserPollResponseAsync; found: {Found}", result != null);
        return result;
    }

    private static PollDto MapToPollDto(Poll poll)
    {
        return new PollDto
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description,
            Type = poll.Type,
            AllowMultipleVotes = poll.AllowMultipleVotes,
            CreatedDate = poll.CreatedDate,
            UpdatedDate = poll.UpdatedDate,
            EndDate = poll.EndDate,
            CreatedByUserId = poll.CreatedByUserId,
            CreatedByUserName = poll.CreatedByUser?.UserName,
            Questions = poll.Questions.OrderBy(q => q.Order).Select(q => new PollQuestionDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Type = q.Type,
                Order = q.Order,
                IsRequired = q.IsRequired,
                Options = q.Options.OrderBy(o => o.Order).Select(o => new PollOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    Order = o.Order,
                    VoteCount = o.VoteCount
                }).ToList()
            }).ToList()
        };
    }

    private static PollResultDto MapToPollResultDto(Poll poll, IEnumerable<PollResponse> responses)
    {
        return new PollResultDto
        {
            PollId = poll.Id,
            Title = poll.Title,
            Description = poll.Description,
            Type = poll.Type,
            TotalResponses = responses.Count(),
            Questions = poll.Questions.OrderBy(q => q.Order).Select(q => new PollQuestionResultDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Type = q.Type,
                Order = q.Order,
                IsRequired = q.IsRequired,
                Options = q.Options.OrderBy(o => o.Order).Select(o => new PollOptionResultDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    Order = o.Order,
                    VoteCount = o.VoteCount,
                    Percentage = responses.Count() > 0 ? (double)o.VoteCount / responses.Count() * 100 : 0
                }).ToList(),
                TextAnswers = responses
                    .SelectMany(r => r.Answers)
                    .Where(a => a.PollQuestionId == q.Id && !string.IsNullOrEmpty(a.TextAnswer))
                    .Select(a => a.TextAnswer!)
                    .ToList(),
                RatingAverage = q.Type == QuestionType.Rating ? responses
                    .SelectMany(r => r.Answers)
                    .Where(a => a.PollQuestionId == q.Id && a.RatingValue.HasValue)
                    .Average(a => a.RatingValue!.Value) : (double?)null
            }).ToList()
        };
    }

    private static PollResponseDto MapToPollResponseDto(PollResponse response)
    {
        return new PollResponseDto
        {
            Id = response.Id,
            PollId = response.PollId,
            UserId = response.UserId,
            RespondedDate = response.RespondedDate,
            Answers = response.Answers.Select(a => new PollAnswerDto
            {
                Id = a.Id,
                PollQuestionId = a.PollQuestionId,
                PollOptionId = a.PollOptionId,
                TextAnswer = a.TextAnswer,
                RatingValue = a.RatingValue
            }).ToList()
        };
    }
}
