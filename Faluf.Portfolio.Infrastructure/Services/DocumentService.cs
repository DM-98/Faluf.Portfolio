using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class DocumentService : Service<Document, DocumentModel, DocumentDTO>
{
	public DocumentService(HttpClient httpClient) : base(httpClient, "documents") { }
}