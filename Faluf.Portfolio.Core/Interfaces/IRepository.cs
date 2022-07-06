using Faluf.Portfolio.Core.DTOs.Response;

namespace Faluf.Portfolio.Core.Interfaces;

public interface IRepository<T, TIN, TOUT> where T : class where TIN : class where TOUT : class
{
	IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync(CancellationToken cancellationToken = default);

	Task<ResponseDTO<TOUT>> GetByIdAsync(int entityId, CancellationToken cancellationToken = default);

	Task<ResponseDTO<TOUT>> CreateAsync(TIN entity, CancellationToken cancellationToken = default);

	Task<ResponseDTO<TOUT>> UpdateAsync(TIN entity, CancellationToken cancellationToken = default);

	Task<ResponseDTO<TOUT>> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}