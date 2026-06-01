using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class PasswordService : IPasswordService
	{
		private readonly PasswordHasher<User> _hasher = new();

		public string HashPassword(User user, string password)
		{
			return _hasher.HashPassword(user, password);
		}

		public bool VerifyPassword(User user, string hashedPassword, string password)
		{
			return _hasher.VerifyHashedPassword(user, hashedPassword, password)
				   == PasswordVerificationResult.Success;
		}
	}
}