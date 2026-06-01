using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Amigurumemi.Contracts.Messaging.Requests.Shared;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using Amigurumemi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[UserAuthorize]
public class ProjectsController : Controller
{
	private readonly ProjectApiService _apiService;
	private readonly YarnApiService _yarnApiService;
	private readonly PatternApiService _patternApiService; 

	public ProjectsController(
		ProjectApiService apiService,
		YarnApiService yarnApiService,
		PatternApiService patternApiService)
	{
		_apiService = apiService;
		_yarnApiService = yarnApiService;
		_patternApiService = patternApiService;
	}

	public async Task<IActionResult> Index(string? name, ProjectStatus? status, int page = 1)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");
		var sessionRole = HttpContext.Session.GetString("Role");

		int currentUserId = string.IsNullOrEmpty(sessionUserId) ? 0 : int.Parse(sessionUserId);

		int? filterUserId = null;
		if (sessionRole != "Admin")
		{
			filterUserId = currentUserId;
		}

		var request = new ProjectGetRequest
		{
			Filter = new ProjectFilterRequest
			{
				Name = name,
				Status = status,
				UserId = filterUserId 
			},
			Pager = new PagerRequest
			{
				Page = page,
				PageSize = 6
			}
		};

		var response = await _apiService.SearchAsync(request);

		return View(response);
	}

	public async Task<IActionResult> Create()
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");

		if (string.IsNullOrEmpty(sessionUserId))
		{
			return RedirectToAction("Login", "Account");
		}

		var model = new ProjectRequest
		{
			StartDate = DateTime.Today,
			UserId = int.Parse(sessionUserId)
		};

		var yarns = await _yarnApiService.GetAllAsync();
		ViewBag.Yarns = yarns;

		int currentUserId = int.Parse(sessionUserId);
		var allPatterns = await _patternApiService.GetAllAsync();
		var userPatterns = allPatterns.Where(p => p.UserId == currentUserId).ToList();

		ViewBag.Patterns = new SelectList(userPatterns, "Id", "Name");

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Create(ProjectRequest request, int[] selectedYarnIds)
	{
		if (!ModelState.IsValid)
		{
			var yarns = await _yarnApiService.GetAllAsync();
			ViewBag.Yarns = yarns;

			var allPatterns = await _patternApiService.GetAllAsync();
			var userPatterns = allPatterns.Where(p => p.UserId == request.UserId).ToList();
			ViewBag.Patterns = new SelectList(userPatterns, "Id", "Name");

			return View(request);
		}

		request.SelectedYarnIds = selectedYarnIds?.ToList() ?? new List<int>();

		await _apiService.CreateAsync(request);
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var project = await _apiService.GetByIdAsync(id);
		if (project == null) return NotFound();

		var allPatterns = await _patternApiService.GetAllAsync();
		var userPatterns = allPatterns.Where(p => p.UserId == project.UserId).ToList();
		ViewBag.Patterns = new SelectList(userPatterns, "Id", "Name", project.PatternId);

		var yarns = await _yarnApiService.GetAllAsync();
		ViewBag.Yarns = yarns;

		ViewBag.CurrentYarnIds = project.YarnIds ?? new List<int>();

		return View(new ProjectRequest
		{
			Name = project.Name,
			PatternId = project.PatternId,
			UserId = project.UserId,
			Status = project.Status,
			Progress = project.Progress,
			StartDate = project.StartDate,
			Notes = project.Notes ?? "",
			EstimatedCost = project.EstimatedCost,
			IsFavorite = project.IsFavorite,
			SelectedYarnIds = project.YarnIds ?? new List<int>()
		});
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, ProjectRequest request, int[] selectedYarnIds)
	{
		var sessionUserId = HttpContext.Session.GetString("UserId");
		var sessionRole = HttpContext.Session.GetString("Role");

		if (sessionRole != "Admin" && !string.IsNullOrEmpty(sessionUserId))
		{
			request.UserId = int.Parse(sessionUserId);
		}

		if (!ModelState.IsValid)
		{
			var allPatterns = await _patternApiService.GetAllAsync();
			var userPatterns = allPatterns.Where(p => p.UserId == request.UserId).ToList();
			ViewBag.Patterns = new SelectList(userPatterns, "Id", "Name", request.PatternId);

			var yarns = await _yarnApiService.GetAllAsync();
			ViewBag.Yarns = yarns;
			ViewBag.CurrentYarnIds = selectedYarnIds?.ToList() ?? new List<int>();

			return View(request);
		}

		request.SelectedYarnIds = selectedYarnIds?.ToList() ?? new List<int>();
		await _apiService.UpdateAsync(id, request);

		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	public async Task<IActionResult> Start(int id)
	{
		await _apiService.StartProjectAsync(id);
		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	public async Task<IActionResult> Complete(int id)
	{
		await _apiService.CompleteProjectAsync(id);
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var project = await _apiService.GetByIdAsync(id);
		if (project == null) return NotFound();
		return View(project);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _apiService.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}


}