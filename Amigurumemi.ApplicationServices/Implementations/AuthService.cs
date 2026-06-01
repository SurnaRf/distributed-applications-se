using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Auth;
using Amigurumemi.Contracts.Messaging.Responses.Auth;
using Amigurumemi.Repositories.Interfaces;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPasswordService _passwordService;
		private readonly ITokenService _tokenService;

		public AuthService(
			IUnitOfWork unitOfWork,
			IPasswordService passwordService,
			ITokenService tokenService)
		{
			_unitOfWork = unitOfWork;
			_passwordService = passwordService;
			_tokenService = tokenService;
		}

		public async Task<AuthTokenResponse> LoginAsync(AuthTokenRequest request)
		{
			var user = (await _unitOfWork.Users.FindAsync(u => u.Username == request.Username))
					   .FirstOrDefault();

			if (user == null)
				throw new Exception("Invalid credentials");

			var isValid = _passwordService.VerifyPassword(user, user.PasswordHash, request.Password);

			if (!isValid)
				throw new Exception("Invalid credentials");

			var token = _tokenService.CreateToken(user);

			return new AuthTokenResponse
			{
				Token = token,
				UserId = user.Id,
				Username = user.Username,
				Role = user.Role,
				Expiration = DateTime.UtcNow.AddHours(1)
			};
		}

		public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
		{
			var user = (await _unitOfWork.Users.FindAsync(u => u.Id == request.UserId)).FirstOrDefault();

			if (user == null)
				throw new Exception("User not found");

			var isCurrentPasswordValid = _passwordService.VerifyPassword(user, user.PasswordHash, request.CurrentPassword);
			if (!isCurrentPasswordValid)
			{
				throw new Exception("Current password is invalid.");
			}

			user.PasswordHash = _passwordService.HashPassword(user, request.NewPassword);

			_unitOfWork.Users.Update(user);

			var result = await _unitOfWork.SaveChangesAsync();

			return true;
		}
	}
}