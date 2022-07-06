using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faluf.Portfolio.API.Controllers;

[Route("subjects")]
[ApiController]
public class SubjectsController : EFControllerBase<Subject, SubjectModel, SubjectDTO>
{
	public SubjectsController(IRepositoryAPI<Subject, SubjectModel, SubjectDTO> repository) : base(repository) { }
}