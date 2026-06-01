using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Requests.Shared;
using Amigurumemi.Contracts.Messaging.Responses.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class PatternService : BaseService, IPatternService
	{
		public PatternService(
			ILogger<PatternService> logger,
			IUnitOfWork unitOfWork)
			: base(logger, unitOfWork)
		{
		}

		public async Task CreateAsync(PatternRequest request)
		{
			try
			{
				var pattern = new Pattern
				{
					Name = request.Name,
					Description = request.Description,
					Difficulty = request.Difficulty,
					Category = request.Category,
					Price = request.Price,
					IsPremium = request.Price > 0,
					PublishedOn = DateTime.UtcNow,
					EstimatedHours = request.EstimatedHours,
					UserId = request.UserId
				};

				await _unitOfWork.Patterns.AddAsync(pattern);
				await _unitOfWork.SaveChangesAsync();

				_logger.LogInformation("Pattern {Name} created successfully.", request.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating pattern {Name}", request.Name);
				throw;
			}
		}

		public async Task<PatternResponse?> GetByIdAsync(int id)
		{
			var pattern = await _unitOfWork.Patterns.GetByIdAsync(id);
			if (pattern == null) return null;

			return new PatternResponse
			{
				Id = pattern.Id,
				Name = pattern.Name,
				Description = pattern.Description,
				Category = pattern.Category,
				Difficulty = pattern.Difficulty,
				Price = pattern.Price,
				IsPremium = pattern.IsPremium,
				EstimatedHours = pattern.EstimatedHours,
				PublishedOn = pattern.PublishedOn,
				UserId = pattern.UserId
			};
		}

		public async Task<IEnumerable<PatternResponse>> GetAllAsync()
		{
			var patterns = await _unitOfWork.Patterns.GetAllAsync();

			return patterns.Select(p => new PatternResponse
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Category = p.Category,
				Difficulty = p.Difficulty,
				Price = p.Price,
				IsPremium = p.IsPremium,
				EstimatedHours = p.EstimatedHours,
				PublishedOn = p.PublishedOn,
				UserId = p.UserId
			});
		}

		public async Task UpdateAsync(int id, PatternRequest request)
		{
			var pattern = await _unitOfWork.Patterns.GetByIdAsync(id);
			if (pattern == null) throw new Exception("Pattern not found");

			pattern.Name = request.Name;
			pattern.Description = request.Description;
			pattern.Category = request.Category;
			pattern.Difficulty = request.Difficulty;
			pattern.Price = request.Price;
			pattern.IsPremium = request.Price > 0;
			pattern.EstimatedHours = request.EstimatedHours; 

			_unitOfWork.Patterns.Update(pattern);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var pattern = await _unitOfWork.Patterns.GetByIdAsync(id);
			if (pattern == null)
				throw new Exception("Pattern not found");

			var allProjects = await _unitOfWork.Projects.GetAllAsync();
			var isPatternUsed = allProjects.Any(p => p.PatternId == id);

			if (isPatternUsed)
			{
				throw new InvalidOperationException("This pattern cannot be deleted because it is currently linked to one or more projects.");
			}

			_unitOfWork.Patterns.Remove(pattern);
			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("Pattern {Id} deleted successfully.", id);
		}

		public async Task<PatternGetResponse> GetAsync(PatternGetRequest request)
		{
			if (request.Pager == null)
			{
				request.Pager = new PagerRequest { Page = 1, PageSize = 5 };
			}

			int currentPage = request.Pager.Page <= 0 ? 1 : request.Pager.Page;
			int pageSize = request.Pager.PageSize <= 0 ? 5 : request.Pager.PageSize;

			var patterns = await _unitOfWork.Patterns.GetAllAsync();
			var query = patterns.AsQueryable();

			if (request.Filter?.UserId != null && request.Filter.UserId > 0)
			{
				query = query.Where(x => x.UserId == request.Filter.UserId);
			}

			if (!string.IsNullOrWhiteSpace(request.Filter?.Name))
			{
				query = query.Where(x => x.Name != null &&
										 x.Name.Contains(request.Filter.Name, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(request.Filter?.Category))
			{
				query = query.Where(x => x.Category != null &&
										 x.Category.Equals(request.Filter.Category, StringComparison.OrdinalIgnoreCase));
			}

			if (request.Filter?.Difficulty != null)
			{
				query = query.Where(x => x.Difficulty == request.Filter.Difficulty);
			}

			var count = query.Count();

			var data = query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.Select(p => new PatternResponse
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Category = p.Category,
					Difficulty = p.Difficulty,
					Price = p.Price,
					IsPremium = p.IsPremium,
					EstimatedHours = p.EstimatedHours,
					PublishedOn = p.PublishedOn,
					UserId = p.UserId
				})
				.ToList();

			return new PatternGetResponse
			{
				Items = data,
				Pager = new PagerResponse
				{
					Page = currentPage,
					PageSize = pageSize,
					Count = count
				},
				Filter = request.Filter
			};
		}
	}
}
