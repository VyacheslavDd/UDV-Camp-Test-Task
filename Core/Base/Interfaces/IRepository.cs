using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base.Interfaces
{
	public interface IRepository<T>
	{
		IQueryable<T> GetAll();
		Task<T?> GetBySelectorAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken);
		IQueryable<T> GetAllWithInclude<TProperty>(Expression<Func<T, TProperty>> includeExpression);
		Task<T?> GetBySelectorWithIncludeAsync<TProperty>(Expression<Func<T, bool>> selector, Expression<Func<T, TProperty>> includeExpression,
			CancellationToken cancellationToken);
		Task<Guid> AddAsync(T entity);
		Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> selector, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateInstructions);
		Task<int> BulkDeleteAsync(Expression<Func<T, bool>> selector);
		Task<Result> NotBulkDeleteAsync(Expression<Func<T, bool>> selector);
		Task SaveChangesAsync();
	}
}
