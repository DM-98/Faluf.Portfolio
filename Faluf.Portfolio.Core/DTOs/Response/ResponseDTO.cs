using System.Net;

namespace Faluf.Portfolio.Core.DTOs.Response;

public class ResponseDTO<T> where T : class
{
	public bool Success { get; set; }

	public string? ErrorMessage { get; set; }

	public string? ExceptionMessage { get; set; }

	public string? InnerExceptionMessage { get; set; }

	public string? ErrorType { get; set; }

	public T? Content { get; set; }
}