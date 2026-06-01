using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Amigurumemi.Repositories.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
		Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
		Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
		void Update(TEntity entity);
		void Remove(TEntity entity);
	}
}
