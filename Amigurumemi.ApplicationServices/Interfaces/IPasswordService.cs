using Amigurumemi.Data.Entities;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IPasswordService
	{
		string HashPassword(User user, string password);
		bool VerifyPassword(User user, string hashedPassword, string password);
	}
}