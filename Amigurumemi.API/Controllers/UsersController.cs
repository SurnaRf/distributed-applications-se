using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Users;
using Microsoft.AspNetCore.Mvc;

namespace Amigurumemi.WebApi.Controllers
{
	/// <summary>
	/// Manages users in the system.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>Create a new user.</summary>
		[HttpPost]
		public async Task<IActionResult> Create(UserRequest request)
		{
			await _userService.CreateAsync(request);
			return Ok();
		}

		/// <summary>Get user by ID.</summary>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var user = await _userService.GetByIdAsync(id);
			return user is null ? NotFound() : Ok(user);
		}

		/// <summary>Get all users.</summary>
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var users = await _userService.GetAllAsync();
			return Ok(users);
		}

		/// <summary>Get user stats by user ID.</summary>
		[HttpGet("{id}/stats")]
		public async Task<IActionResult> GetStats(int id)
		{
			try
			{
				var stats = await _userService.GetUserStatsAsync(id);
				return Ok(stats);
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		/// <summary>Update user.</summary>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, UserRequest request)
		{
			await _userService.UpdateAsync(id, request);
			return Ok();
		}

		/// <summary>Delete user.</summary>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _userService.DeleteAsync(id);
			return Ok();
		}

		/// <summary>
		/// Searches and filters users with pagination (by username and email).
		/// </summary>
		[HttpPost("search")]
		public async Task<IActionResult> Search([FromBody] UserGetRequest request)
		{
			var result = await _userService.GetAsync(request);
			return Ok(result);
		}
	}
}