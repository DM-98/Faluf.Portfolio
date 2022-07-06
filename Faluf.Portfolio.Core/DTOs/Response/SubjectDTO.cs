using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Response;

public class SubjectDTO
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public bool IsInactive { get; set; }

	public DateTime CreatedDate { get; init; }

	public DateTime UpdatedDate { get; set; }
}