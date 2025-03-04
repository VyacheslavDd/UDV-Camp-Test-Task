using Core.Base.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
	public abstract class BaseRepository<T>(DbContext dbContext, DbSet<T> dbSet) : IRepository<T> where T: class
	{
		protected readonly DbContext _dbContext = dbContext;
		protected readonly DbSet<T> _dbSet = dbSet;

		public virtual IQueryable<T> GetAll()
		{
			return _dbSet;
		}

		public virtual async Task<T?> GetBySelectorAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken)
		{
			return await _dbSet.Where(selector).FirstOrDefaultAsync(cancellationToken);
		}

		public virtual async Task<Guid> AddAsync(T entity)
		{
			var createdEntity = await _dbSet.AddAsync(entity);
			await SaveChangesAsync();
			return (Guid)createdEntity.Property("Id").CurrentValue;
		}

		public virtual async Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> selector, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateInstructions)
		{
			return await _dbSet.Where(selector).ExecuteUpdateAsync(updateInstructions);
		}

		public virtual async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> selector)
		{
			return await _dbSet.Where(selector).ExecuteDeleteAsync();
		}

		public virtual async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
	}
}
