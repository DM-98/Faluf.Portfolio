using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Faluf.Portfolio.Core.Domain;

[Table("ApplicationUsers")]
public class ApplicationUser : IdentityUser<int>
{
	public string IPAddress { get; set; } = null!;

	[NotMapped]
	public virtual ICollection<Subject>? Subjects { get; set; }

	[NotMapped]
	public virtual ICollection<Log>? Logs { get; set; }

	[NotMapped]
	public virtual ICollection<Document>? Documents { get; set; }

	[Timestamp]
	public byte[] RowVersion { get; set; } = null!;

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
	public DateTime CreatedDate { get; init; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
	public DateTime UpdatedDate { get; set; }

	public ApplicationUser()
	{
		CreatedDate = DateTime.Now;
		UpdatedDate = DateTime.Now;
	}
}