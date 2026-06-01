using Amigurumemi.Data;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly DbSet<TEntity> _set;

		public Repository(IDbContext context)
		{
			_set = context.Set<TEntity>();
		}

		public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
			=> await _set.FindAsync([id], cancellationToken);

		public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
			=> await _set.ToListAsync(cancellationToken);

		public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
			=> await _set.Where(predicate).ToListAsync(cancellationToken);

		public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
			=> await _set.AddAsync(entity, cancellationToken);

		public void Update(TEntity entity)
			=> _set.Update(entity);

		public void Remove(TEntity entity)
			=> _set.Remove(entity);
	}
}
