using Amigurumemi.Contracts.Messaging.Requests.Auth;
using Amigurumemi.Contracts.Messaging.Requests.Users;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
	private readonly AuthApiService _authApiService;

	public AccountController(AuthApiService authApiService)
	{
		_authApiService = authApiService;
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View(new AuthTokenRequest());
	}

	[HttpPost]
	public async Task<IActionResult> Login(AuthTokenRequest request)
	{
		if (!ModelState.IsValid) return View(request);

		var result = await _authApiService.LoginAsync(request);
		if (result == null)
		{
			ModelState.AddModelError("", "Invalid username or password.");
			return View(request);
		}

		HttpContext.Session.SetString("Token", result.Token);
		HttpContext.Session.SetString("Username", result.Username);
		HttpContext.Session.SetString("UserId", result.UserId.ToString());
		HttpContext.Session.SetString("Role", result.Role);

		return RedirectToAction("Index", "Home");
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View(new UserRequest());
	}

	[HttpPost]
	public async Task<IActionResult> Register(UserRequest request)
	{
		if (!ModelState.IsValid) return View(request);

		var isSuccess = await _authApiService.RegisterAsync(request);
		if (!isSuccess)
		{
			ModelState.AddModelError("", "Username or email are already taken.");
			return View(request);
		}

		return RedirectToAction(nameof(Login));
	}

	public IActionResult Logout()
	{
		HttpContext.Session.Clear();
		return RedirectToAction(nameof(Login));
	}

	[HttpGet]
	public IActionResult ChangePassword()
	{
		var userIdStr = HttpContext.Session.GetString("UserId");
		if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login");

		var model = new ChangePasswordRequest
		{
			UserId = int.Parse(userIdStr)
		};

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
	{
		System.Diagnostics.Debug.WriteLine($"Current: {model.CurrentPassword}, New: {model.NewPassword}");

		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var userIdStr = HttpContext.Session.GetString("UserId");
		if (string.IsNullOrEmpty(userIdStr))
		{
			return RedirectToAction("Login");
		}

		model.UserId = int.Parse(userIdStr);

		var isSuccess = await _authApiService.ChangePasswordAsync(model);

		if (isSuccess)
		{
			TempData["SuccessMessage"] = "Password changed successfully!";
			return RedirectToAction("MyProfile", "Users");
		}

		ModelState.AddModelError("", "Unsuccessful change. Check if the current password is correct.");
		return View(model);
	}
}