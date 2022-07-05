using Faluf.Portfolio.Core.DTOs.Response;

namespace Faluf.Portfolio.Core.Interfaces;

public interface IRepository<T> where T : class
{
	IAsyncEnumerable<ResponseDTO<T>> GetAllAsync(CancellationToken cancellationToken = default);

	Task<ResponseDTO<T>> GetByIdAsync(int entityId, CancellationToken cancellationToken = default);

	Task<ResponseDTO<T>> CreateAsync(T entity, CancellationToken cancellationToken = default);

	Task<ResponseDTO<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default);

	Task<ResponseDTO<T>> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}