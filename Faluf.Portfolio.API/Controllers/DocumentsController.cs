using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("documents")]
[ApiController]
public class DocumentsController : EFControllerBase<Document>
{
	public DocumentsController(IRepositoryAPI<Document> repository) : base(repository) { }
}