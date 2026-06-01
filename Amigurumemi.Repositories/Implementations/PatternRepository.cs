using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	internal class PatternRepository : Repository<Pattern>, IPatternRepository
	{
		public PatternRepository(IDbContext context) : base(context)
		{
		}
	}
}
