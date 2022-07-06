using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class DocumentModel
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[MaxLength(25, ErrorMessage = "The {0} must contain {1} characters or less!")]
	[Display(Name = "Title")]
	public string Title { get; set; } = null!;

	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Content")]
	public string Content { get; set; } = null!;

	[Required(ErrorMessage = "At least one tag is required!")]
	[MaxLength(200, ErrorMessage = "The {0} must contain {1} characters or less!")]
	[Display(Name = "Tags")]
	public string Tags { get; set; } = null!;

	[Display(Name = "Mark document as finished")]
	public bool IsFinished { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; }
}