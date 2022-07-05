using System.ComponentModel.DataAnnotations.Schema;

namespace Faluf.Portfolio.Core.Domain;

[Table("Subjects")]
public class Subject : BaseEntity
{
	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public bool IsInactive { get; set; }

	[NotMapped]
	public virtual ICollection<Document>? Documents { get; set; }

	public int UserId { get; init; }

	[NotMapped]
	[ForeignKey("UserId")]
	public virtual ApplicationUser? User { get; init; }
}