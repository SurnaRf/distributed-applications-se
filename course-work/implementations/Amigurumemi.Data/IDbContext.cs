using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Data
{
	public interface IDbContext
	{
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
