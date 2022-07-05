using Faluf.Portfolio.Core.DTOs.Response;
using System.Linq.Expressions;

namespace Faluf.Portfolio.Core.Interfaces;

public interface IRepositoryAPI<T> : IRepository<T> where T : class
{
    IAsyncEnumerable<ResponseDTO<T>> FindAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);
}