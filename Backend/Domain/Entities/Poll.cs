using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Poll
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public PollType Type { get; set; }

    public bool IsActive { get; set; } = true;

    public bool AllowMultipleVotes { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }

    // Navigation properties
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public ICollection<PollQuestion> Questions { get; set; } = new List<PollQuestion>();

    public ICollection<PollResponse> Responses { get; set; } = new List<PollResponse>();
}

public enum PollType
{
    YesNo = 1,
    MultipleChoice = 2,
    Survey = 3
}
