using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Repositories.Implementations
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(IDbContext context) : base(context)
		{
		}
	}
}
