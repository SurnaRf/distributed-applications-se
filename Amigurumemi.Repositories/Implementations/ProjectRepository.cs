using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	public class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository(IDbContext context) : base(context)
		{
		}
	}
}
