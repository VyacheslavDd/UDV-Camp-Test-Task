using Core.Base.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
	public abstract class BaseService<T>(IRepository<T> repository, ILogger logger) : IService<T> where T : class
	{
		private readonly Func<Guid, Expression<Func<T, bool>>> _idSelectorExpression = (Guid id) => e => (e as Entity<Guid>).Id == id;

		protected readonly IRepository<T> _repository = repository;
		protected readonly ILogger _logger = logger;

		public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _repository.GetAll().ToListAsync(cancellationToken);
		}

		public virtual async Task<Result<Guid>> AddAsync(T entity)
		{
			var validationResult = await ValidateAsync(entity);
			if (!validationResult.IsSuccess) return Result.Failure<Guid>(validationResult.Error);
			var answer = await _repository.AddAsync(entity);
			return Result.Success(answer);
		}


		public virtual async Task SaveChangesAsync()
		{
			await _repository.SaveChangesAsync();
		}

		public virtual Task<Result> NotBulkUpdateAsync(Guid entityId, T updateRequest)
		{
			return Task.FromResult(Result.Success());
		}

		public virtual async Task<Result> NotBulkDeleteAsync(Guid entityId)
		{
			return await _repository.NotBulkDeleteAsync(_idSelectorExpression(entityId));
		}

		public virtual async Task<T?> GetByGuidAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _repository.GetBySelectorAsync(_idSelectorExpression(id), cancellationToken);
		}

		public virtual async Task<Result<string>> BulkDeleteAsync(Guid entityId)
		{
			var deletedRowsCount = await _repository.BulkDeleteAsync(_idSelectorExpression(entityId));
			return Result.Success($"Удалено строк: {deletedRowsCount}");
		}

		public virtual Task<Result<string>> BulkUpdateAsync(Guid entityId, T updateRequest)
		{
			return Task.FromResult(Result.Success("Обновлено строк: 0"));
		}
		public virtual Task<Result<string>> ValidateAsync(T entity, bool fromUpdate=false)
		{
			return Task.FromResult(Result.Success("Success"));
		}
	}
}
