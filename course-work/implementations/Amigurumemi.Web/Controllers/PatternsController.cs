using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Requests.Shared;
using Amigurumemi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

[UserAuthorize]
public class PatternsController : Controller
{
	private readonly PatternApiService _service;

	public PatternsController(
		PatternApiService service)
	{
		_service = service;
	}

	public async Task<IActionResult> Index(
		string? name,
		string? category,
		DifficultyLevel? difficulty,
		string? orderBy,         
		bool sortAsc = true,      
		int page = 1)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");
		var sessionRole = HttpContext.Session.GetString("Role");

		int currentUserId = string.IsNullOrEmpty(sessionUserId) ? 0 : int.Parse(sessionUserId);

		int? filterUserId = null;
		if (sessionRole != "Admin")
		{
			filterUserId = currentUserId;
		}

		var response =
			await _service.SearchAsync(
				new PatternGetRequest
				{
					Filter = new PatternFilterRequest
					{
						Name = name,
						Category = category,
						Difficulty = difficulty,
						UserId = filterUserId
					},
					Pager = new PagerRequest
					{
						Page = page,
						PageSize = 5
					},
					OrderBy = orderBy,    
					SortAsc = sortAsc     
				});

		return View(response);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(PatternRequest request)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");

		if (!string.IsNullOrEmpty(sessionUserId))
		{
			request.UserId = int.Parse(sessionUserId);
		}

		if (!ModelState.IsValid)
			return View(request);

		await _service.CreateAsync(request);

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var pattern =
			await _service.GetByIdAsync(id);

		if (pattern == null)
			return NotFound();

		return View(new PatternRequest
		{
			Name = pattern.Name,
			Description = pattern.Description,
			Category = pattern.Category,
			Difficulty = pattern.Difficulty,
			Price = pattern.Price,
			EstimatedHours = pattern.EstimatedHours,
			UserId = pattern.UserId
		});
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, PatternRequest request)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");
		if (!string.IsNullOrEmpty(sessionUserId))
		{
			request.UserId = int.Parse(sessionUserId);
		}

		if (!ModelState.IsValid)
			return View(request);

		await _service.UpdateAsync(id, request);

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var pattern =
			await _service.GetByIdAsync(id);

		return View(pattern);
	}

	[HttpPost]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		try
		{
			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
		catch (Exception ex)
		{
			var pattern = await _service.GetByIdAsync(id);

			ViewBag.ErrorMessage = ex.Message;

			return View("Delete", pattern);
		}
	}
}