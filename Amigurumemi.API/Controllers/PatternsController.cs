using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace Amigurumemi.Api.Controllers
{
	/// <summary>
	/// Manages crochet/knitting patterns.
	/// Patterns define templates used to create projects.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class PatternsController : ControllerBase
	{
		private readonly IPatternService _patternService;

		public PatternsController(IPatternService patternService)
		{
			_patternService = patternService;
		}

		/// <summary>
		/// Creates a new pattern.
		/// </summary>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] PatternRequest request)
		{
			await _patternService.CreateAsync(request);
			return Ok(new { message = "Pattern created successfully" });
		}

		/// <summary>
		/// Gets a pattern by ID.
		/// </summary>
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _patternService.GetByIdAsync(id);

			if (result == null)
				return NotFound();

			return Ok(result);
		}

		/// <summary>
		/// Gets all patterns.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var result = await _patternService.GetAllAsync();
			return Ok(result);
		}

		/// <summary>
		/// Updates an existing pattern.
		/// </summary>
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, [FromBody] PatternRequest request)
		{
			try
			{
				await _patternService.UpdateAsync(id, request);
				return Ok(new { message = "Pattern updated successfully" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		/// <summary>
		/// Deletes a pattern.
		/// </summary>
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _patternService.DeleteAsync(id);
				return Ok(new { message = "Pattern deleted successfully" });
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		[HttpPost("search")]
		public async Task<ActionResult<PatternGetResponse>>
		Search([FromBody] PatternGetRequest request)
		{
			return Ok(
				await _patternService.GetAsync(request));
		}
	}
}