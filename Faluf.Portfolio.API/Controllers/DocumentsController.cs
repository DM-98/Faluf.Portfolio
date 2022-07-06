using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("documents")]
[ApiController]
public class DocumentsController : EFControllerBase<Document, DocumentModel, DocumentDTO>
{
	public DocumentsController(IRepositoryAPI<Document, DocumentModel, DocumentDTO> repository) : base(repository) { }
}