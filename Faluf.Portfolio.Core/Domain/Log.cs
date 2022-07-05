using System.ComponentModel.DataAnnotations.Schema;

namespace Faluf.Portfolio.Core.Domain;

[Table("Logs")]
public class Log : BaseEntity
{
	public string Description { get; init; } = null!;

	public int UserId { get; init; }

	[NotMapped]
	[ForeignKey("UserId")]
	public virtual ApplicationUser? User { get; init; }
}