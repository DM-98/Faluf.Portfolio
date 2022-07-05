using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Controllers;

public abstract class EFControllerBase<T> : ControllerBase where T : class
{
	private readonly IRepositoryAPI<T> repository;

	public EFControllerBase(IRepositoryAPI<T> repository)
	{
		this.repository = repository;
	}

	[HttpGet]
	public virtual async IAsyncEnumerable<ResponseDTO<T>> GetAllAsync()
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		await foreach (ResponseDTO<T> entity in repository.GetAllAsync(cts.Token))
		{
			yield return entity;
		}
	}

	[HttpGet("{id}")]
	public virtual async Task<ActionResult<ResponseDTO<T>>> GetByIdAsync(int id)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<T> result = await repository.GetByIdAsync(id, cts.Token);

		if (result.Success)
		{
			return Ok(result);
		}
		else
		{
			if (result.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(result);
			}
			else if (result.ErrorType is nameof(EntryPointNotFoundException))
			{
				return NotFound(result);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, result);
			}
		}
	}

	[HttpPost]
	public virtual async Task<ActionResult<ResponseDTO<T>>> PostAsync(T entity)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<T> result = await repository.CreateAsync(entity, cts.Token);

		if (result.Success)
		{
			return CreatedAtAction(nameof(GetByIdAsync), new { entityId = (int)result.Content!.GetType().GetProperty("Id")!.GetValue(result.Content)! }, entity);
		}
		else
		{
			if (result.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(result);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, result);
			}
		}
	}

	[HttpPut]
	public virtual async Task<ActionResult<ResponseDTO<T>>> PutAsync(T entity)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<T> result = await repository.UpdateAsync(entity, cts.Token);

		if (result.Success)
		{
			return Ok(result);
		}
		else
		{
			if (result.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(result);
			}
			else if (result.ErrorType is nameof(DbUpdateConcurrencyException))
			{
				return Conflict(result);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, result);
			}
		}
	}

	[HttpDelete]
	public virtual async Task<ActionResult<ResponseDTO<T>>> DeleteAsync(T entity)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<T> result = await repository.DeleteAsync(entity, cts.Token);

		if (result.Success)
		{
			return Ok(result);
		}
		else
		{
			if (result.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(result);
			}
			else if (result.ErrorType is nameof(DbUpdateConcurrencyException))
			{
				return Conflict(result);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, result);
			}
		}
	}
}