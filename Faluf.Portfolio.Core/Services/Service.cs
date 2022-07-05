using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Faluf.Portfolio.Core.Services;

public abstract class Service<T> : IRepository<T> where T : class
{
	private readonly HttpClient httpClient;
	private readonly string controllerName;

	public Service(HttpClient httpClient, string controllerName)
	{
		this.httpClient = httpClient;
		this.controllerName = controllerName;
	}

	public virtual async IAsyncEnumerable<ResponseDTO<T>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		HttpResponseMessage result = await httpClient.GetAsync(controllerName);
		result.EnsureSuccessStatusCode();
		IAsyncEnumerable<ResponseDTO<T>>? responseDTO = await JsonSerializer.DeserializeAsync<IAsyncEnumerable<ResponseDTO<T>>>(await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

		await foreach (ResponseDTO<T> entity in responseDTO!.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			yield return entity;
		}
	}

	public virtual async Task<ResponseDTO<T>> GetByIdAsync(int entityId, CancellationToken cancellationToken = default)
	{
		string uri = controllerName + "/" + WebUtility.UrlEncode(entityId.ToString());

		try
		{
			HttpResponseMessage result = await httpClient.GetAsync(uri, cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<T> response = JsonSerializer.Deserialize<ResponseDTO<T>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return response;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<T>> CreateAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpResponseMessage result = await httpClient.PostAsync(controllerName, new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"), cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<T> responseDTO = JsonSerializer.Deserialize<ResponseDTO<T>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return responseDTO;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpResponseMessage result = await httpClient.PutAsync(controllerName, new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"), cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<T> response = JsonSerializer.Deserialize<ResponseDTO<T>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return response;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<T>> DeleteAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpRequestMessage request = new(HttpMethod.Delete, controllerName);
			request.Content = new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
			response.EnsureSuccessStatusCode();
			ResponseDTO<T> responseDTO = JsonSerializer.Deserialize<ResponseDTO<T>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return responseDTO;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}
}