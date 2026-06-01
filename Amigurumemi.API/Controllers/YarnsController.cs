using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Manages yarns used in projects.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class YarnsController : ControllerBase
{
	private readonly IYarnService _yarnService;

	public YarnsController(IYarnService yarnService)
	{
		_yarnService = yarnService;
	}

	/// <summary>
	/// Creates a new yarn.
	/// </summary>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] YarnRequest request)
	{
		await _yarnService.CreateAsync(request);
		return Ok(new { message = "Yarn created successfully" });
	}

	/// <summary>
	/// Gets yarn by ID.
	/// </summary>
	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(int id)
	{
		var result = await _yarnService.GetByIdAsync(id);

		if (result == null)
			return NotFound();

		return Ok(result);
	}

	/// <summary>
	/// Gets all yarns.
	/// </summary>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll()
	{
		var result = await _yarnService.GetAllAsync();
		return Ok(result);
	}

	/// <summary>
	/// Updates an existing yarn.
	/// </summary>
	[HttpPut("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(int id, [FromBody] YarnRequest request)
	{
		try
		{
			await _yarnService.UpdateAsync(id, request);
			return Ok(new { message = "Yarn updated successfully" });
		}
		catch (Exception ex)
		{
			return NotFound(new { error = ex.Message });
		}
	}

	/// <summary>
	/// Deletes a yarn.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _yarnService.DeleteAsync(id);
			return Ok(new { message = "Yarn deleted successfully" });
		}
		catch (Exception ex)
		{
			return NotFound(new { error = ex.Message });
		}
	}

	/// <summary>
	/// Gets total cost of yarns for a project.
	/// </summary>
	[HttpGet("project/{projectId:int}/total-cost")]
	public async Task<IActionResult> GetTotalCostByProject(int projectId)
	{
		var result = await _yarnService.GetTotalCostByProjectAsync(projectId);
		return Ok(new { projectId, totalCost = result });
	}

	/// <summary>
	/// Gets yarns filtered by color.
	/// </summary>
	[HttpGet("color/{color}")]
	public async Task<IActionResult> GetByColor(string color)
	{
		var result = await _yarnService.GetByColorAsync(color);
		return Ok(result);
	}

	/// <summary>
	/// Gets the most expensive yarn.
	/// </summary>
	[HttpGet("most-expensive")]
	public async Task<IActionResult> GetMostExpensive()
	{
		var result = await _yarnService.GetMostExpensiveAsync();

		if (result == null)
			return NotFound();

		return Ok(result);
	}

	/// <summary>
	/// Assigns yarn to a project.
	/// </summary>
	[HttpPost("{yarnId:int}/assign/{projectId:int}")]
	public async Task<IActionResult> AssignToProject(int yarnId, int projectId)
	{
		try
		{
			await _yarnService.AssignToProjectAsync(yarnId, projectId);
			return Ok(new { message = "Yarn assigned to project" });
		}
		catch (Exception ex)
		{
			return NotFound(new { error = ex.Message });
		}
	}

	/// <summary>
	/// Searches and filters yarns with pagination support (by name and brand).
	/// </summary>
	[HttpPost("search")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> Search([FromBody] YarnGetRequest request)
	{
		var result = await _yarnService.GetAsync(request);
		return Ok(result);
	}
}