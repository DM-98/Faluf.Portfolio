namespace Faluf.Portfolio.Core.Interfaces;

public interface ILocalStorageService
{
	Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

	Task SetAsync<T>(string key, T value, CancellationToken cancellationToken);

	Task DeleteAsync(string key, CancellationToken cancellationToken);

	Task ClearAllAsync(CancellationToken cancellationToken);
}