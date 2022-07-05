using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Faluf.Portfolio.Infrastructure.EFRepository;

public class EFRepository<T, TDbContext> : IRepositoryAPI<T> where T : class where TDbContext : DbContext
{
	private readonly TDbContext context; // Entity Frameworks Unit of Work
	private readonly DbSet<T> table; // Entity Frameworks Repository

	public EFRepository(TDbContext context)
	{
		this.context = context;
		table = context.Set<T>();
	}

	public async IAsyncEnumerable<ResponseDTO<T>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (T entity in table.AsAsyncEnumerable())
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
				yield break;
			}

			yield return new ResponseDTO<T> { Success = true, Content = entity };
		}
	}

	public async Task<ResponseDTO<T>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		try
		{
			T? requestedEntity = await table.FindAsync(new object[] { id }, cancellationToken);

			if (requestedEntity is null)
			{
				return new ResponseDTO<T> { Success = false, ErrorMessage = $"Requested {typeof(T).Name} was not found with the provided ID.", ErrorType = nameof(EntryPointNotFoundException) };
			}

			return new ResponseDTO<T> { Success = true, Content = requestedEntity };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Server Error", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async IAsyncEnumerable<ResponseDTO<T>> FindAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, [EnumeratorCancellation] CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> query = table;

		if (predicate is not null)
		{
			query = query.Where(predicate);
		}

		if (includeProperties is not null)
		{
			foreach (Expression<Func<T, object>> includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
		}

		if (orderBy is not null)
		{
			await foreach (T entity in orderBy(query).AsAsyncEnumerable())
			{
				if (cancellationToken.IsCancellationRequested)
				{
					yield return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
					yield break;
				}

				yield return new ResponseDTO<T> { Success = true, Content = entity };
			}
		}
		else
		{
			await foreach (T entity in query.AsAsyncEnumerable())
			{
				if (cancellationToken.IsCancellationRequested)
				{
					yield return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
					yield break;
				}

				yield return new ResponseDTO<T> { Success = true, Content = entity };
			}
		}
	}

	public async Task<ResponseDTO<T>> CreateAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			await table.AddAsync(entity, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			return new ResponseDTO<T> { Success = true, Content = entity };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Server Error", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			context.Entry(entity).State = EntityState.Modified;
			await context.SaveChangesAsync(cancellationToken);

			return new ResponseDTO<T> { Success = true, Content = entity };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (DbUpdateConcurrencyException ex)
		{
			T? dbEntity = await table.FindAsync(entity.GetType().GetProperty("Id")?.GetValue(entity));

			if (dbEntity is null)
			{
				return new ResponseDTO<T> { Success = false, ErrorMessage = $"Concurrency conflict upon editing - {nameof(T)} was deleted from the database before your request.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException) };
			}

			return new ResponseDTO<T> { Success = false, ErrorMessage = "Concurrency conflict upon editing - check the new database data and try again.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException), Content = dbEntity };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Server Error", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<T>> DeleteAsync(T entity, CancellationToken cancellationToken = default)
	{
		try
		{
			table.Remove(entity);
			await context.SaveChangesAsync(cancellationToken);

			return new ResponseDTO<T> { Success = true, Content = entity };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (DbUpdateConcurrencyException ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Concurrency conflict upon deletion - {nameof(T)} was already deleted from the database.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<T> { Success = false, ErrorMessage = $"Unhandled Server Error", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}
}