using System.ComponentModel.DataAnnotations.Schema;

namespace Faluf.Portfolio.Core.Domain;

[Table("Documents")]
public class Document : BaseEntity
{
	public string Title { get; set; } = null!;

	public string Content { get; set; } = null!;

	public string Tags { get; set; } = null!;

	public bool IsFinished { get; set; }

	public int SubjectId { get; init; }

	[NotMapped]
	[ForeignKey("SubjectId")]
	public virtual Subject? Subject { get; init; }
}