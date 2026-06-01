using Amigurumemi.Contracts.Messaging.Requests.Auth;
using Amigurumemi.Contracts.Messaging.Responses.Auth;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IAuthService
	{
		Task<AuthTokenResponse> LoginAsync(AuthTokenRequest request);
		Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
	}
}