using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("subjects")]
[ApiController]
public class SubjectsController : EFControllerBase<Subject>
{
	public SubjectsController(IRepositoryAPI<Subject> repository) : base(repository) { }
}