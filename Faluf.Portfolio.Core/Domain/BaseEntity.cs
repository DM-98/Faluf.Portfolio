using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.Domain;

public abstract class BaseEntity
{
	[Key]
	public int Id { get; init; }

	[Timestamp]
	public byte[]? RowVersion { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
	public DateTime CreatedDate { get; init; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
	public DateTime UpdatedDate { get; set; }

	public BaseEntity()
	{
		CreatedDate = DateTime.Now;
		UpdatedDate = DateTime.Now;
	}
}