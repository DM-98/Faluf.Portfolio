using Faluf.Portfolio.Core.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Faluf.Portfolio.Infrastructure.Services;

public class LocalStorageService : ILocalStorageService
{
	private readonly IJSRuntime jsRuntime;

	public LocalStorageService(IJSRuntime jsRuntime)
	{
		this.jsRuntime = jsRuntime;
	}

	public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
	{
		string json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);

		if (string.IsNullOrWhiteSpace(json))
		{
			return default;
		}

		return JsonSerializer.Deserialize<T>(json);
	}

	public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
	{
		await jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, JsonSerializer.Serialize(value));
	}

	public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
	{
		await jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
	}

	public async Task ClearAllAsync(CancellationToken cancellationToken = default)
	{
		await jsRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);
	}
}