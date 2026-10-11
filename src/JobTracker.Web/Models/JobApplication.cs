using System.ComponentModel.DataAnnotations;

namespace JobTracker.Web.Models;

public class JobApplication
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Company is required")]
    [StringLength(100)]
    public string Company { get; set; } = "";

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100)]
    public string Position { get; set; } = "";

    [StringLength(100)]
    public string Location { get; set; } = "";

    [Url(ErrorMessage = "Enter a valid URL")]
    public string? JobUrl { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;
    public DateTime? AppliedOn { get; set; }
    public DateTime? FollowUpOn { get; set; }
}