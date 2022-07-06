using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Controllers;

public abstract class EFControllerBase<T, TIN, TOUT> : ControllerBase where T : class where TIN : class where TOUT : class
{
	private readonly IRepositoryAPI<T, TIN, TOUT> repository;

	public EFControllerBase(IRepositoryAPI<T, TIN, TOUT> repository)
	{
		this.repository = repository;
	}

	[HttpGet]
	public async IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync()
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		await foreach (ResponseDTO<TOUT> entity in repository.GetAllAsync(cts.Token))
		{
			yield return entity;
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ResponseDTO<TOUT>>> GetByIdAsync(int id)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<TOUT> result = await repository.GetByIdAsync(id, cts.Token);

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
			else if (result.ErrorType is nameof(ArgumentNullException))
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
	public async Task<ActionResult<ResponseDTO<TOUT>>> PostAsync(TIN entityModel)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<TOUT> resultDTO = await repository.CreateAsync(entityModel, cts.Token);

		if (resultDTO.Success)
		{
			return CreatedAtAction(nameof(GetByIdAsync), new { id = (int)resultDTO.Content!.GetType().GetProperty("Id")?.GetValue(resultDTO.Content)! }, resultDTO.Content);
		}
		else
		{
			if (resultDTO.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(resultDTO);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, resultDTO);
			}
		}
	}

	[HttpPut]
	public async Task<ActionResult<ResponseDTO<TOUT>>> PutAsync(TIN entity)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<TOUT> resultDTO = await repository.UpdateAsync(entity, cts.Token);

		if (resultDTO.Success)
		{
			return Ok(resultDTO);
		}
		else
		{
			if (resultDTO.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(resultDTO);
			}
			else if (resultDTO.ErrorType is nameof(DbUpdateConcurrencyException))
			{
				return Conflict(resultDTO);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, resultDTO);
			}
		}
	}

	[HttpDelete]
	public async Task<ActionResult<ResponseDTO<TOUT>>> DeleteAsync(T entity)
	{
		using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

		ResponseDTO<TOUT> resultDTO = await repository.DeleteAsync(entity, cts.Token);

		if (resultDTO.Success)
		{
			return Ok(resultDTO);
		}
		else
		{
			if (resultDTO.ErrorType is nameof(OperationCanceledException))
			{
				return UnprocessableEntity(resultDTO);
			}
			else if (resultDTO.ErrorType is nameof(DbUpdateConcurrencyException))
			{
				return Conflict(resultDTO);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, resultDTO);
			}
		}
	}
}