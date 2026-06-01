using Amigurumemi.Data.Entities;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(User user);
	}
}