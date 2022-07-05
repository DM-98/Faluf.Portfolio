using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class UserService : Service<ApplicationUser>
{
	private readonly HttpClient httpClient;

	public UserService(HttpClient httpClient) : base(httpClient, "users")
	{
		this.httpClient = httpClient;
	}
}