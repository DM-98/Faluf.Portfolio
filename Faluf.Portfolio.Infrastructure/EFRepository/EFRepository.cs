using AutoMapper;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Faluf.Portfolio.Infrastructure.EFRepository;

public class EFRepository<T, TIN, TOUT, TDbContext> : IRepositoryAPI<T, TIN, TOUT> where T : class where TIN : class where TOUT : class where TDbContext : DbContext
{
	private readonly IMapper mapper;
	private readonly TDbContext context; // Entity Frameworks Unit of Work
	private readonly DbSet<T> table; // Entity Frameworks Repository

	public EFRepository(TDbContext context, IMapper mapper)
	{
		this.context = context;
		this.mapper = mapper;
		table = context.Set<T>();
	}

	public async IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (T entity in table.AsAsyncEnumerable())
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
				yield break;
			}

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			yield return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
	}

	public async Task<ResponseDTO<TOUT>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		try
		{
			T requestedEntity = await table.FindAsync(new object[] { id }, cancellationToken) ?? throw new ArgumentNullException();

			TOUT requestedEntityDTO = mapper.Map<TOUT>(requestedEntity);

			return new ResponseDTO<TOUT> { Success = true, Content = requestedEntityDTO };
		}
		catch (ArgumentNullException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Requested {typeof(TIN).Name} was not found with the provided ID.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(ArgumentNullException) };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async IAsyncEnumerable<ResponseDTO<TOUT>> FindAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, [EnumeratorCancellation] CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
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
					yield return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
					yield break;
				}

				TOUT entityDTO = mapper.Map<TOUT>(entity);

				yield return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
			}
		}
		else
		{
			await foreach (T entity in query.AsAsyncEnumerable())
			{
				if (cancellationToken.IsCancellationRequested)
				{
					yield return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ErrorType = nameof(OperationCanceledException) };
					yield break;
				}

				TOUT entityDTO = mapper.Map<TOUT>(entity);

				yield return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
			}
		}
	}

	public async Task<ResponseDTO<TOUT>> CreateAsync(TIN entityModel, CancellationToken cancellationToken = default)
	{
		T entity = mapper.Map<T>(entityModel);

		try
		{
			await table.AddAsync(entity, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}

	public async Task<ResponseDTO<TOUT>> UpdateAsync(TIN entityModel, CancellationToken cancellationToken = default)
	{
		T entity = mapper.Map<T>(entityModel);

		try
		{
			context.Entry(entity).State = EntityState.Modified;
			await context.SaveChangesAsync(cancellationToken);

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (DbUpdateConcurrencyException ex)
		{
			T dbEntity = await table.FindAsync(entity.GetType().GetProperty("Id")?.GetValue(entity)) ?? throw new ArgumentNullException(null, ex.InnerException);

			TOUT dbEntityDTO = mapper.Map<TOUT>(dbEntity);

			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Concurrency conflict upon editing - check the new database data and try again.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException), Content = dbEntityDTO };
		}
		catch (ArgumentNullException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Concurrency conflict upon editing - {typeof(TIN).Name} was deleted from the database before your request.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException) };
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
			table.Remove(entity);
			await context.SaveChangesAsync(cancellationToken);

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Your request was cancelled due to timeout.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(OperationCanceledException) };
		}
		catch (DbUpdateConcurrencyException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Concurrency conflict upon deletion - {typeof(TIN).Name} was already deleted from the database.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(DbUpdateConcurrencyException) };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Unhandled server error.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = nameof(Exception) };
		}
	}
}