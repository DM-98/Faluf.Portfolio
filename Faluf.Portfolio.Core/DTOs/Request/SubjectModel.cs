using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class SubjectModel
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Subject name")]
	[StringLength(25, MinimumLength = 2, ErrorMessage = "The {0} must be between {1} and {2} characters long!")]
	public string Name { get; set; } = null!;

	[Display(Name = "Subject description")]
	[MaxLength(100, ErrorMessage = "The {0} must contain {1} characters or less!")]
	public string? Description { get; set; }

	[Display(Name = "Mark subject as inactive (hide it from the portfolio)")]
	public bool IsInactive { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; }
}