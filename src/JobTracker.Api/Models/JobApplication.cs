using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobTracker.Api.Models;

public class JobApplication
{
    public int Id { get; set; }

    [JsonIgnore]
    public string UserId { get; set; } = "";

    [Required, StringLength(100)]
    public string Company { get; set; } = "";

    [Required, StringLength(100)]
    public string Position { get; set; } = "";

    [StringLength(100)]
    public string Location { get; set; } = "";

    [Url]
    public string? JobUrl { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;
    public DateTime? AppliedOn { get; set; }
    public DateTime? FollowUpOn { get; set; }
}