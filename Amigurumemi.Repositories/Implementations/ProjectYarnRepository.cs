using Amigurumemi.Data;
using Amigurumemi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	internal class ProjectYarnRepository : Repository<ProjectYarn>, IProjectYarnRepository
	{
		public ProjectYarnRepository(IDbContext context) : base(context)
		{
		}
	}
}
