using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Response;

public class UserDTO
{
	public int Id { get; set; }

	public string UserName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string? PhoneNumber { get; set; }

	public DateTime CreatedDate { get; init; }

	public DateTime UpdatedDate { get; set; }
}