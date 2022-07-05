using Faluf.Portfolio.Core.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Faluf.Portfolio.Infrastructure.Services;

public class LocalStorageService : ILocalStorageService
{
	private readonly IJSRuntime jsRunTime;

	public LocalStorageService(IJSRuntime jsRunTime)
	{
		this.jsRunTime = jsRunTime;
	}

	public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
	{
		string json = await jsRunTime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);

		if (string.IsNullOrWhiteSpace(json))
		{
			return default;
		}

		return JsonSerializer.Deserialize<T>(json);
	}

	public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken)
	{
		await jsRunTime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, JsonSerializer.Serialize(value));
	}

	public async Task DeleteAsync(string key, CancellationToken cancellationToken)
	{
		await jsRunTime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
	}

	public async Task ClearAllAsync(CancellationToken cancellationToken)
	{
		await jsRunTime.InvokeVoidAsync("localStorage.clear", cancellationToken);
	}
}