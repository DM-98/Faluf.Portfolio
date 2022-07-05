using Microsoft.AspNetCore.Components.Authorization;

namespace Faluf.Portfolio.Blazor.ApplicationState;

public class AuthStateProvider : AuthenticationStateProvider
{
	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		throw new NotImplementedException();
	}
}