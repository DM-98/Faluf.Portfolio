namespace Faluf.Portfolio.Core.Interfaces;

public interface IAuthenticationService<T> where T : class
{
	Task<T> GetTokenAsync(string scope);
}