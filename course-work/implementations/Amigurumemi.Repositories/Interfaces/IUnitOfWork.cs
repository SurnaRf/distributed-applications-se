using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IUserRepository Users { get; }
		IPatternRepository Patterns { get; }
		IProjectRepository Projects { get; }
		IYarnRepository Yarns { get; }
		IProjectYarnRepository ProjectYarns { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
