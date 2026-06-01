using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Auth;
using Amigurumemi.Contracts.Messaging.Responses.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amigurumemi.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] AuthTokenRequest request)
		{
			try
			{
				var result = await _authService.LoginAsync(request);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
		}

		[HttpPost("change-password")]
		[Authorize] 
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
		{
			try
			{
				var result = await _authService.ChangePasswordAsync(request);
				if (result) return Ok(new { message = "Password changed successfully." });
				return BadRequest("Something went wrong.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}