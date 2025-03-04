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
		Task<Guid> AddAsync(T entity);
		Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> selector, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateInstructions);
		Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> selector);
		Task SaveChangesAsync();
	}
}
