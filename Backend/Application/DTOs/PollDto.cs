using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public class PollDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public PollType Type { get; set; }

    public bool AllowMultipleVotes { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int CreatedByUserId { get; set; }

    public string? CreatedByUserName { get; set; }

    public List<PollQuestionDto> Questions { get; set; } = new();
}

public class PollQuestionDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [Required]
    public QuestionType Type { get; set; }

    public int Order { get; set; }

    public bool IsRequired { get; set; }

    public List<PollOptionDto> Options { get; set; } = new();
}

public class PollOptionDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string OptionText { get; set; } = string.Empty;

    public int Order { get; set; }

    public int VoteCount { get; set; }
}

public class CreatePollDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public PollType Type { get; set; }

    public bool AllowMultipleVotes { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one question is required")]
    public List<CreatePollQuestionDto> Questions { get; set; } = new();
}

public class CreatePollQuestionDto
{
    [Required]
    [StringLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [Required]
    public QuestionType Type { get; set; }

    public bool IsRequired { get; set; } = true;

    public List<CreatePollOptionDto>? Options { get; set; }
}

public class CreatePollOptionDto
{
    [Required]
    [StringLength(200)]
    public string OptionText { get; set; } = string.Empty;
}

public class UpdatePollDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public PollType Type { get; set; }

    public bool AllowMultipleVotes { get; set; }

    public DateTime? EndDate { get; set; }
}

public class PollResponseDto
{
    public int Id { get; set; }
    public int PollId { get; set; }
    public int? UserId { get; set; }
    public DateTime RespondedDate { get; set; }
    public List<PollAnswerDto> Answers { get; set; } = new();
}

public class PollAnswerDto
{
    public int Id { get; set; }
    public int PollQuestionId { get; set; }
    public int? PollOptionId { get; set; }
    public string? TextAnswer { get; set; }
    public int? RatingValue { get; set; }
}

public class SubmitPollResponseDto
{
    [Required]
    public int PollId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one answer is required")]
    public List<SubmitPollAnswerDto> Answers { get; set; } = new();
}

public class SubmitPollAnswerDto
{
    [Required]
    public int PollQuestionId { get; set; }

    public int? PollOptionId { get; set; }

    public string? TextAnswer { get; set; }

    public int? RatingValue { get; set; }
}

public class PollResultDto
{
    public int PollId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PollType Type { get; set; }
    public int TotalResponses { get; set; }
    public List<PollQuestionResultDto> Questions { get; set; } = new();
}

public class PollQuestionResultDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public int Order { get; set; }
    public bool IsRequired { get; set; }
    public List<PollOptionResultDto> Options { get; set; } = new();
    public List<string> TextAnswers { get; set; } = new();
    public double? RatingAverage { get; set; }
}

public class PollOptionResultDto
{
    public int Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public int Order { get; set; }
    public int VoteCount { get; set; }
    public double Percentage { get; set; }
}
