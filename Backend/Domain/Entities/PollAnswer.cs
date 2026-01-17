using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PollAnswer
{
    public int Id { get; set; }

    [StringLength(1000)]
    public string? TextAnswer { get; set; }

    public int? RatingValue { get; set; }

    public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public int PollResponseId { get; set; }
    public PollResponse PollResponse { get; set; } = null!;

    public int PollQuestionId { get; set; }
    public PollQuestion PollQuestion { get; set; } = null!;

    public int? PollOptionId { get; set; }
    public PollOption? PollOption { get; set; }
}
