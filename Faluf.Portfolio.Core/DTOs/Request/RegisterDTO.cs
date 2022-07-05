using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class RegisterDTO
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Email")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Username")]
	public string Username { get; set; } = null!;

	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Password")]
	public string Password { get; set; } = null!;
}