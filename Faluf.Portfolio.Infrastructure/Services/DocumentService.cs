using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class DocumentService : Service<Document>
{
	private readonly HttpClient httpClient;

	public DocumentService(HttpClient httpClient) : base(httpClient, "documents")
	{
		this.httpClient = httpClient;
	}
}