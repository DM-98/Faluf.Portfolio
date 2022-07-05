using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class DocumentDTO
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Title")]
	public string Title { get; set; } = null!;

	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Content")]
	public string Content { get; set; } = null!;

	[Required(ErrorMessage = "At least one tag is required!")]
	[Display(Name = "Tags")]
	public string Tags { get; set; } = null!;

	[Display(Name = "Mark document as finished")]
	public bool IsFinished { get; set; }

	[Required(ErrorMessage = "Choose a {0}!")]
	[Display(Name = "Subject")]
	public int SubjectId { get; set; }
}