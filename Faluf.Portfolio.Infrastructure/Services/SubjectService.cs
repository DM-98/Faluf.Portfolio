using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class SubjectService : Service<Subject, SubjectModel, SubjectDTO>
{
	private readonly HttpClient httpClient;

	public SubjectService(HttpClient httpClient) : base(httpClient, "subjects")
	{
		this.httpClient = httpClient;
	}
}