using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class LogService : Service<Log>
{
	private readonly HttpClient httpClient;

	public LogService(HttpClient httpClient) : base(httpClient, "logs")
	{
		this.httpClient = httpClient;
	}
}