using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("users")]
[ApiController]
public class UsersController : EFControllerBase<ApplicationUser>
{
	public UsersController(IRepositoryAPI<ApplicationUser> repository) : base(repository) { }
}