using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PollOption
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string OptionText { get; set; } = string.Empty;

    public int Order { get; set; }

    public int VoteCount { get; set; } = 0;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public int PollQuestionId { get; set; }
    public PollQuestion PollQuestion { get; set; } = null!;

    public ICollection<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
}
