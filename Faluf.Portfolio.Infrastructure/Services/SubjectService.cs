using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class SubjectService : Service<Subject>
{
	private readonly HttpClient httpClient;

	public SubjectService(HttpClient httpClient) : base(httpClient, "subjects")
	{
		this.httpClient = httpClient;
	}
}