using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Amigurumemi.Api.Controllers
{
	/// <summary>
	/// Manages projects (crochet/knitting works).
	/// Includes creation, updates, progress tracking and lifecycle actions.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;

		public ProjectsController(IProjectService projectService)
		{
			_projectService = projectService;
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] ProjectRequest request)
		{
			await _projectService.CreateAsync(request);
			return Ok(new { message = "Project created successfully" });
		}

		/// <summary>
		/// Gets a project by its ID.
		/// </summary>
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _projectService.GetByIdAsync(id);

			if (result == null)
				return NotFound();

			return Ok(result);
		}

		/// <summary>
		/// Gets all projects.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var result = await _projectService.GetAllAsync();
			return Ok(result);
		}

		/// <summary>
		/// Updates an existing project.
		/// </summary>
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, [FromBody] ProjectRequest request)
		{
			try
			{
				await _projectService.UpdateAsync(id, request);
				return Ok(new { message = "Project updated successfully" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Deletes a project.
		/// </summary>
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _projectService.DeleteAsync(id);
				return Ok(new { message = "Project deleted successfully" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Starts a project (changes status to InProgress).
		/// </summary>
		[HttpPost("{id:int}/start")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Start(int id)
		{
			try
			{
				await _projectService.StartProjectAsync(id);
				return Ok(new { message = "Project started" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Marks a project as completed.
		/// </summary>
		[HttpPost("{id:int}/complete")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Complete(int id)
		{
			try
			{
				await _projectService.CompleteProjectAsync(id);
				return Ok(new { message = "Project completed" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Updates project progress (0-100%).
		/// </summary>
		[HttpPatch("{id:int}/progress")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateProgress(int id, [FromQuery] int progress)
		{
			try
			{
				await _projectService.UpdateProgressAsync(id, progress);
				return Ok(new { message = "Progress updated" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Searches and filters projects with pagination support.
		/// </summary>
		[HttpPost("search")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Search([FromBody] ProjectGetRequest request)
		{
			var result = await _projectService.GetAsync(request);
			return Ok(result);
		}
	}
}