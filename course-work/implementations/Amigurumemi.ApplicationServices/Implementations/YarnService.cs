using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Shared;
using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Contracts.Messaging.Responses.Yarns;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class YarnService : BaseService, IYarnService
	{
		public YarnService(
			ILogger<YarnService> logger,
			IUnitOfWork unitOfWork)
			: base(logger, unitOfWork)
		{
		}

		public async Task CreateAsync(YarnRequest request)
		{
			try
			{
				var yarn = new Yarn
				{
					Name = request.Name,
					Brand = request.Brand,
					Color = request.Color,
					Weight = request.Weight,
					Price = request.Price,
					UserId = request.UserId
				};

				await _unitOfWork.Yarns.AddAsync(yarn);
				await _unitOfWork.SaveChangesAsync();

				_logger.LogInformation("Yarn {Name} created successfully.", request.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating yarn {Name}", request.Name);
				throw;
			}
		}

		public async Task<YarnResponse?> GetByIdAsync(int id)
		{
			var yarn = await _unitOfWork.Yarns.GetByIdAsync(id);

			if (yarn == null)
			{
				_logger.LogWarning("Yarn {Id} not found.", id);
				return null;
			}

			return new YarnResponse
			{
				Id = yarn.Id,
				Name = yarn.Name,
				Brand = yarn.Brand,
				Color = yarn.Color,
				Weight = yarn.Weight,
				Price = yarn.Price
			};
		}

		public async Task<IEnumerable<YarnResponse>> GetAllAsync()
		{
			var yarns = await _unitOfWork.Yarns.GetAllAsync();

			return yarns.Select(y => new YarnResponse
			{
				Id = y.Id,
				Name = y.Name,
				Brand = y.Brand,
				Color = y.Color,
				Weight = y.Weight,
				Price = y.Price
			});
		}

		public async Task UpdateAsync(int id, YarnRequest request)
		{
			var yarn = await _unitOfWork.Yarns.GetByIdAsync(id);

			if (yarn == null)
				throw new Exception("Yarn not found");

			yarn.Name = request.Name;
			yarn.Brand = request.Brand;
			yarn.Color = request.Color;
			yarn.Weight = request.Weight;
			yarn.Price = request.Price;

			_unitOfWork.Yarns.Update(yarn);
			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("Yarn {Id} updated successfully.", id);
		}

		public async Task DeleteAsync(int id)
		{
			var yarn = await _unitOfWork.Yarns.GetByIdAsync(id);

			if (yarn == null)
				throw new Exception("Yarn not found");

			var projectYarns = await _unitOfWork.ProjectYarns.FindAsync(py => py.YarnId == id);
			foreach (var py in projectYarns)
			{
				_unitOfWork.ProjectYarns.Remove(py);
			}

			_unitOfWork.Yarns.Remove(yarn);
			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("Yarn {Id} deleted successfully.", id);
		}

		public async Task<decimal> GetTotalCostByProjectAsync(int projectId)
		{
			var projectYarns = await _unitOfWork.ProjectYarns.FindAsync(x => x.ProjectId == projectId);
			var yarnIds = projectYarns.Select(py => py.YarnId).ToList();

			var yarns = await _unitOfWork.Yarns.GetAllAsync();
			return yarns.Where(y => yarnIds.Contains(y.Id)).Sum(x => x.Price);
		}

		public async Task<IEnumerable<YarnResponse>> GetByColorAsync(string color)
		{
			var yarns = await _unitOfWork.Yarns.FindAsync(x => x.Color == color);
			return yarns.Select(Map);
		}

		public async Task<YarnResponse?> GetMostExpensiveAsync()
		{
			var yarns = await _unitOfWork.Yarns.GetAllAsync();
			var yarn = yarns.OrderByDescending(x => x.Price).FirstOrDefault();

			return yarn == null ? null : Map(yarn);
		}

		public async Task AssignToProjectAsync(int yarnId, int projectId)
		{
			var yarn = await _unitOfWork.Yarns.GetByIdAsync(yarnId);
			var project = await _unitOfWork.Projects.GetByIdAsync(projectId);

			if (yarn == null)
				throw new Exception("Yarn not found");

			if (project == null)
				throw new Exception("Project not found");

			var existingAssignment = await _unitOfWork.ProjectYarns
				.FindAsync(py => py.ProjectId == projectId && py.YarnId == yarnId);

			if (!existingAssignment.Any())
			{
				var projectYarn = new ProjectYarn
				{
					ProjectId = projectId,
					YarnId = yarnId
				};

				await _unitOfWork.ProjectYarns.AddAsync(projectYarn);
				await _unitOfWork.SaveChangesAsync();
			}
		}

		private static YarnResponse Map(Yarn y)
		{
			return new YarnResponse
			{
				Id = y.Id,
				Name = y.Name,
				Brand = y.Brand,
				Color = y.Color,
				Weight = y.Weight,
				Price = y.Price
			};
		}

		public async Task<YarnGetResponse> GetAsync(YarnGetRequest request)
		{
			if (request.Pager == null)
			{
				request.Pager = new PagerRequest { Page = 1, PageSize = 5 };
			}

			int currentPage = request.Pager.Page <= 0 ? 1 : request.Pager.Page;
			int pageSize = request.Pager.PageSize <= 0 ? 5 : request.Pager.PageSize;

			var yarns = await _unitOfWork.Yarns.GetAllAsync();
			var query = yarns.AsQueryable();

			if (request.Filter?.UserId != null && request.Filter.UserId > 0)
			{
				query = query.Where(x => x.UserId == request.Filter.UserId);
			}

			if (request.Filter != null && !string.IsNullOrWhiteSpace(request.Filter.Name))
			{
				query = query.Where(x => x.Name != null &&
										 x.Name.Contains(request.Filter.Name, StringComparison.OrdinalIgnoreCase));
			}

			if (request.Filter != null && !string.IsNullOrWhiteSpace(request.Filter.Brand))
			{
				query = query.Where(x => x.Brand != null &&
										 x.Brand.Contains(request.Filter.Brand, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(request.OrderBy))
			{
				switch (request.OrderBy.ToLower())
				{
					case "name":
						query = request.SortAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
						break;
					case "brand":
						query = request.SortAsc ? query.OrderBy(x => x.Brand) : query.OrderByDescending(x => x.Brand);
						break;
					case "color":
						query = request.SortAsc ? query.OrderBy(x => x.Color) : query.OrderByDescending(x => x.Color);
						break;
					case "price":
						query = request.SortAsc ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price);
						break;
					default:
						query = query.OrderBy(x => x.Id);
						break;
				}
			}
			else
			{
				query = query.OrderBy(x => x.Id);
			}

			var totalCount = query.Count();

			var pagedData = query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.Select(y => new YarnResponse
				{
					Id = y.Id,
					Name = y.Name,
					Brand = y.Brand,
					Color = y.Color,
					Weight = y.Weight,
					Price = y.Price
				})
				.ToList();

			return new YarnGetResponse
			{
				Items = pagedData,
				Pager = new PagerResponse
				{
					Page = currentPage,
					PageSize = pageSize,
					Count = totalCount
				},
				Filter = request.Filter,
				OrderBy = request.OrderBy ?? "Id",
				SortAsc = request.SortAsc
			};
		}
	}
}