using Faluf.Portfolio.Core.DTOs.Response;
using System.Linq.Expressions;

namespace Faluf.Portfolio.Core.Interfaces;

public interface IRepositoryAPI<T, TIN, TOUT> : IRepository<T, TIN, TOUT> where T : class where TIN : class where TOUT : class
{
    IAsyncEnumerable<ResponseDTO<TOUT>> FindAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);
}