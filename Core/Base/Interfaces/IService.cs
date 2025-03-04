using CSharpFunctionalExtensions;
using System.Linq.Expressions;

namespace Core.Base.Interfaces
{
	public interface IService<T>
	{
		Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
		Task<T?> GetByGuidAsync(Guid id, CancellationToken cancellationToken);
		Task<Result<Guid>> AddAsync(T entity);
		Task SaveChangesAsync();
		Task<Result> NotBulkUpdateAsync(Guid entityId, T updateRequest);
		Task<Result> NotBulkDeleteAsync(Guid entityId);
		Task<Result<string>> BulkDeleteAsync(Guid entityId);
		Task<Result<string>> BulkUpdateAsync(Guid entityId, T updateRequest);
		abstract Task<Result<string>> ValidateAsync(T entity, bool fromUpdate);
	}
}
