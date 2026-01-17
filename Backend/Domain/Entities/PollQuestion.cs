using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PollQuestion
{
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [Required]
    public QuestionType Type { get; set; }

    public int Order { get; set; }

    public bool IsRequired { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public ICollection<PollOption> Options { get; set; } = new List<PollOption>();

    public ICollection<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
}

public enum QuestionType
{
    YesNo = 1,
    MultipleChoice = 2,
    Checkbox = 3,
    Text = 4,
    Rating = 5
}
