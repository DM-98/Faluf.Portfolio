using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("users")]
[ApiController]
public class UsersController : EFControllerBase<ApplicationUser, UserModel, UserDTO>
{
	public UsersController(IRepositoryAPI<ApplicationUser, UserModel, UserDTO> repository) : base(repository) { }
}