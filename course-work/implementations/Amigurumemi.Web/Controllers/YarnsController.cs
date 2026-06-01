using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Amigurumemi.Contracts.Messaging.Responses.Yarns;
using Microsoft.AspNetCore.Mvc;

[UserAuthorize]
public class YarnsController : Controller
{
	private readonly YarnApiService _yarnApiService;

	public YarnsController(YarnApiService yarnApiService)
	{
		_yarnApiService = yarnApiService;
	}
	public async Task<IActionResult> Index(
		string? name,
		string? brand,
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

		var request = new YarnGetRequest
		{
			Filter = new YarnFilterRequest
			{
				Name = name,
				Brand = brand,
				UserId = filterUserId
			},
			Pager = new Amigurumemi.Contracts.Messaging.Requests.Shared.PagerRequest
			{
				Page = page,
				PageSize = 5
			},
			OrderBy = orderBy,    
			SortAsc = sortAsc     
		};

		var response = await _yarnApiService.SearchAsync(request);

		if (response == null)
		{
			response = new YarnGetResponse
			{
				Items = new List<YarnResponse>(),
				Filter = request.Filter
			};
		}

		return View(response);
	}

	public IActionResult Create()
	{
		return View(new YarnRequest());
	}

	[HttpPost]
	public async Task<IActionResult> Create(YarnRequest request)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");
		if (!string.IsNullOrEmpty(sessionUserId))
		{
			request.UserId = int.Parse(sessionUserId);
		}

		if (!ModelState.IsValid) return View(request);

		await _yarnApiService.CreateAsync(request);
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var yarn = await _yarnApiService.GetByIdAsync(id);
		if (yarn == null) return NotFound();

		return View(new YarnRequest
		{
			Name = yarn.Name,
			Brand = yarn.Brand,
			Color = yarn.Color,
			Weight = yarn.Weight,
			Price = yarn.Price,
			ProjectId = yarn.ProjectId
		});
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, YarnRequest request)
	{
		if (!ModelState.IsValid) return View(request);

		await _yarnApiService.UpdateAsync(id, request);
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Delete(int id)
	{
		var yarn = await _yarnApiService.GetByIdAsync(id);
		if (yarn == null) return NotFound();

		return View(yarn);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _yarnApiService.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}
}