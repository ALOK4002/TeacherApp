using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PollResponse
{
    public int Id { get; set; }

    public DateTime RespondedDate { get; set; } = DateTime.UtcNow;

    public string? UserIpAddress { get; set; }

    public string? UserAgent { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public int? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
}
