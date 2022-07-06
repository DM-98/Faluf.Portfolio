using System.ComponentModel.DataAnnotations;

namespace Faluf.Portfolio.Core.DTOs.Request;

public class LoginModel
{
	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Email")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "{0} cannot be empty!")]
	[Display(Name = "Password")]
	public string Password { get; set; } = null!;

	[Display(Name = "Keep me logged in")]
	public bool RememberMe { get; set; }
}