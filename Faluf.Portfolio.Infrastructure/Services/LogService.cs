using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class LogService : Service<Log, Log, Log>
{
	private readonly HttpClient httpClient;

	public LogService(HttpClient httpClient) : base(httpClient, "logs")
	{
		this.httpClient = httpClient;
	}
}