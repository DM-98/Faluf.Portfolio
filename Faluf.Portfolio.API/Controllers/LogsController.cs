using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("logs")]
[ApiController]
public class LogsController : EFControllerBase<Log>
{
	public LogsController(IRepositoryAPI<Log> repository) : base(repository) { }
}