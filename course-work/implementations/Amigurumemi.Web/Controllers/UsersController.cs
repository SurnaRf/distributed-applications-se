using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Users;
using Microsoft.AspNetCore.Mvc;

public class UsersController : Controller
{
	private readonly UserApiService _userApiService;

	public UsersController(UserApiService userApiService)
	{
		_userApiService = userApiService;
	}

	private bool IsAdmin()
	{
		var role = HttpContext.Session.GetString("Role");
		return role == "Admin";
	}

	public async Task<IActionResult> Index(string? username, string? email, int page = 1)
	{
		if (!IsAdmin()) return Forbid();

		var request = new UserGetRequest
		{
			Filter = new UserFilterRequest
			{
				Username = username,
				Email = email
			},
			Pager = new Amigurumemi.Contracts.Messaging.Requests.Shared.PagerRequest
			{
				Page = page,
				PageSize = 5 
			}
		};

		var response = await _userApiService.SearchAsync(request);

		if (response == null)
		{
			response = new UsersGetResponse
			{
				Items = new List<UserResponse>(),
				Filter = request.Filter
			};
		}

		return View(response);
	}

	public async Task<IActionResult> Details(int id)
	{
		if (!IsAdmin()) return Forbid();

		var user = await _userApiService.GetByIdAsync(id);
		if (user == null) return NotFound();

		ViewBag.Stats = await _userApiService.GetUserStatsAsync(id);

		return View(user);
	}

	public async Task<IActionResult> Edit(int id)
	{
		if (!IsAdmin()) return Forbid();

		var user = await _userApiService.GetByIdAsync(id);
		if (user == null) return NotFound();

		return View(new UserRequest
		{
			Username = user.Username,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Password = "" 
		});
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, UserRequest request)
	{
		if (!IsAdmin()) return Forbid();

		if (!ModelState.IsValid) return View(request);

		await _userApiService.UpdateAsync(id, request);
		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	public async Task<IActionResult> Delete(int id)
	{
		if (!IsAdmin()) return Forbid();

		await _userApiService.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> MyProfile()
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");

		if (string.IsNullOrEmpty(sessionUserId))
		{
			return RedirectToAction("Login", "Account");
		}

		int currentUserId = int.Parse(sessionUserId);

		var user = await _userApiService.GetByIdAsync(currentUserId);
		if (user == null) return NotFound();

		ViewBag.Stats = await _userApiService.GetUserStatsAsync(currentUserId);

		return View("Details", user);
	}
}