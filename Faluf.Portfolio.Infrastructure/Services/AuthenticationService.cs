using Faluf.Portfolio.Core.Interfaces;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Faluf.Portfolio.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService<TokenResponse>
{
	private readonly HttpClient httpClient;

	public record IdentityServerSettings
	{
		public static string DiscoveryURI { get; set; } = "https://localhost:7097";

		public static string ClientName { get; set; } = "m2m.client";

		public static string ClientPassword { get; set; } = "SuperSecretPassword";

		public static bool UseHTTPS { get; set; } = true;
	}

	public AuthenticationService(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public async Task<TokenResponse> GetTokenAsync(string scope)
	{
		DiscoveryDocumentResponse discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync(IdentityServerSettings.DiscoveryURI);

		if (discoveryDocumentResponse.IsError)
		{
			throw new Exception("Discovery document not found.", discoveryDocumentResponse.Exception);
		}

		ClientCredentialsTokenRequest clientCredentialsTokenRequest = new();
		clientCredentialsTokenRequest.Address = discoveryDocumentResponse.TokenEndpoint;
		clientCredentialsTokenRequest.ClientId = IdentityServerSettings.ClientName;
		clientCredentialsTokenRequest.ClientSecret = IdentityServerSettings.ClientPassword;
		clientCredentialsTokenRequest.GrantType = IdentityModel.OidcConstants.GrantTypes.ClientCredentials;
		clientCredentialsTokenRequest.Scope = scope;

		TokenResponse tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

		if (tokenResponse.IsError)
		{
			throw new Exception("Could not get token from the client credentials request.", tokenResponse.Exception);
		}

		return tokenResponse;
	}
}