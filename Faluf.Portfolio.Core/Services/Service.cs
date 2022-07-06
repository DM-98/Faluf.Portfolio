using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Faluf.Portfolio.Core.Services;

public abstract class Service<T, TIN, TOUT> : IRepository<T, TIN, TOUT> where T : class where TIN : class where TOUT : class
{
	private readonly HttpClient httpClient;
	private readonly string controllerName;

	public Service(HttpClient httpClient, string controllerName)
	{
		this.httpClient = httpClient;
		this.controllerName = controllerName;
	}

	public virtual async IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		HttpResponseMessage result = await httpClient.GetAsync(controllerName);
		result.EnsureSuccessStatusCode();
		IAsyncEnumerable<ResponseDTO<TOUT>>? responseDTO = await JsonSerializer.DeserializeAsync<IAsyncEnumerable<ResponseDTO<TOUT>>>(await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

		await foreach (ResponseDTO<TOUT> entity in responseDTO!.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			yield return entity;
		}
	}

	public virtual async Task<ResponseDTO<TOUT>> GetByIdAsync(int entityId, CancellationToken cancellationToken = default)
	{
		string uri = controllerName + "/" + WebUtility.UrlEncode(entityId.ToString());

		try
		{
			HttpResponseMessage result = await httpClient.GetAsync(uri, cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<TOUT> response = JsonSerializer.Deserialize<ResponseDTO<TOUT>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return response;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<TOUT>> CreateAsync(TIN entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpResponseMessage result = await httpClient.PostAsync(controllerName, new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"), cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<TOUT> responseDTO = JsonSerializer.Deserialize<ResponseDTO<TOUT>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return responseDTO;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<TOUT>> UpdateAsync(TIN entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpResponseMessage result = await httpClient.PutAsync(controllerName, new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"), cancellationToken);
			result.EnsureSuccessStatusCode();
			ResponseDTO<TOUT> response = JsonSerializer.Deserialize<ResponseDTO<TOUT>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return response;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}

	public virtual async Task<ResponseDTO<TOUT>> DeleteAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			HttpRequestMessage request = new(HttpMethod.Delete, controllerName);
			request.Content = new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
			response.EnsureSuccessStatusCode();
			ResponseDTO<TOUT> responseDTO = JsonSerializer.Deserialize<ResponseDTO<TOUT>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

			return responseDTO;
		}
		catch (HttpRequestException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP Request Error: {ex.Message} | Status Code: {ex.StatusCode} | Details: {ex.InnerException!.Message}", ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled Error: {ex.Message} | Details: {ex.InnerException!.Message}", ErrorType = nameof(Exception) };
		}
	}
}