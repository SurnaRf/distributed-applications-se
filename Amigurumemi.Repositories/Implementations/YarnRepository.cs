using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	public class YarnRepository : Repository<Yarn>, IYarnRepository
	{
		public YarnRepository(IDbContext context) : base(context)
		{
		}
	}
}
