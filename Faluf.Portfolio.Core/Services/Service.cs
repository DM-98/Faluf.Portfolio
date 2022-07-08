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

	public async IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		HttpResponseMessage result = await httpClient.GetAsync(controllerName, cancellationToken);

		if (result.IsSuccessStatusCode)
		{
			IAsyncEnumerable<ResponseDTO<TOUT>> responseDTO = await JsonSerializer.DeserializeAsync<IAsyncEnumerable<ResponseDTO<TOUT>>>(await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) ?? throw new ArgumentNullException();
		
			await foreach (ResponseDTO<TOUT> entity in responseDTO)
			{
				yield return entity;
			}
		}
		else
		{
			yield return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "HTTP request failed.", ExceptionMessage = result.ReasonPhrase };
		}
	}

	public async Task<ResponseDTO<TOUT>> GetByIdAsync(int entityId, CancellationToken cancellationToken = default)
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
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP request failed.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<TOUT>> CreateAsync(TIN entity, CancellationToken cancellationToken = default)
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
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP request failed.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<TOUT>> UpdateAsync(TIN entity, CancellationToken cancellationToken = default)
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
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP request failed.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<TOUT>> DeleteAsync(T entity, CancellationToken cancellationToken = default)
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
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"HTTP request failed.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(HttpRequestException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}
}