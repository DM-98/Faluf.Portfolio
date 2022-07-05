using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class SubjectDTO
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Subject name")]
	public string Name { get; set; } = null!;

	[Display(Name = "Subject description (optional)")]
	public string? Description { get; set; }

	[Display(Name = "Mark subject as inactive (hide it from the portfolio)")]
	public bool IsInactive { get; set; }
}