using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class LogDTO
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Description")]
	public string Description { get; set; } = null!;

	[Required(ErrorMessage = "Log could not be created - user is not logged in!")]
	[Display(Name = "User")]
	public int UserId { get; init; }
}