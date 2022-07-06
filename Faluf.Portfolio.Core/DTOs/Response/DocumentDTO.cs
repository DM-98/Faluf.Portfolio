using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Response;

public class DocumentDTO
{
	public int Id { get; set; }

	public string Title { get; set; } = null!;

	public string Content { get; set; } = null!;

	public string Tags { get; set; } = null!;

	public bool IsFinished { get; set; }

	public DateTime CreatedDate { get; init; }

	public DateTime UpdatedDate { get; set; }
}