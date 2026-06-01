using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class ProjectService : BaseService, IProjectService
	{
		public ProjectService(
			ILogger<ProjectService> logger,
			IUnitOfWork unitOfWork)
			: base(logger, unitOfWork)
		{
		}

		public async Task CreateAsync(ProjectRequest request)
		{
			try
			{
				var project = new Project
				{
					Name = request.Name,
					PatternId = request.PatternId,
					UserId = request.UserId,
					Status = request.Status,
					Progress = request.Progress,
					StartDate = request.StartDate,
					Notes = request.Notes,
					EstimatedCost = request.EstimatedCost,
					IsFavorite = request.IsFavorite
				};

				await _unitOfWork.Projects.AddAsync(project);
				await _unitOfWork.SaveChangesAsync(); 

				if (request.SelectedYarnIds != null && request.SelectedYarnIds.Any())
				{
					foreach (var yarnId in request.SelectedYarnIds)
					{
						await _unitOfWork.ProjectYarns.AddAsync(new ProjectYarn
						{
							ProjectId = project.Id,
							YarnId = yarnId
						});
					}
					await _unitOfWork.SaveChangesAsync();
				}

				_logger.LogInformation("Project {Name} created successfully with selected yarns.", request.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating project {Name}", request.Name);
				throw;
			}
		}
		public async Task<ProjectResponse?> GetByIdAsync(int id)
		{
			var project = await _unitOfWork.Projects.GetByIdAsync(id);

			if (project == null)
			{
				_logger.LogWarning("Project {Id} not found.", id);
				return null;
			}

			var projectYarns = await _unitOfWork.ProjectYarns.GetAllAsync();
			var yarns = await _unitOfWork.Yarns.GetAllAsync();

			var currentProjectYarnIds = projectYarns.Where(py => py.ProjectId == id).Select(py => py.YarnId).ToList();

			return new ProjectResponse
			{
				Id = project.Id,
				Name = project.Name,
				Status = project.Status,
				Progress = project.Progress,
				StartDate = project.StartDate,
				FinishDate = project.FinishDate,
				EstimatedCost = project.EstimatedCost,
				IsFavorite = project.IsFavorite,
				UserId = project.UserId,
				PatternId = project.PatternId,

				YarnIds = currentProjectYarnIds,
				YarnNames = yarns.Where(y => currentProjectYarnIds.Contains(y.Id)).Select(y => y.Name).ToList()
			};
		}

		public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
		{
			var projects = await _unitOfWork.Projects.GetAllAsync();

			return projects.Select(p => new ProjectResponse
			{
				Id = p.Id,
				Name = p.Name,
				Status = p.Status,
				Progress = p.Progress,
				StartDate = p.StartDate,
				FinishDate = p.FinishDate,
				EstimatedCost = p.EstimatedCost,
				IsFavorite = p.IsFavorite,
				UserId = p.UserId,
				PatternId = p.PatternId
			});
		}

		public async Task UpdateAsync(int id, ProjectRequest request)
		{
			var project = await _unitOfWork.Projects.GetByIdAsync(id);

			if (project == null)
				throw new Exception("Project not found");

			project.Name = request.Name;
			project.PatternId = request.PatternId;
			project.Status = request.Status;
			project.Progress = request.Progress;
			project.StartDate = request.StartDate;
			project.Notes = request.Notes;
			project.EstimatedCost = request.EstimatedCost;
			project.IsFavorite = request.IsFavorite;

			_unitOfWork.Projects.Update(project);

			var allProjectYarns = await _unitOfWork.ProjectYarns.GetAllAsync();
			var yarnsToRemove = allProjectYarns.Where(py => py.ProjectId == id).ToList();

			foreach (var py in yarnsToRemove)
			{
				_unitOfWork.ProjectYarns.Remove(py);
			}

			if (request.SelectedYarnIds != null && request.SelectedYarnIds.Any())
			{
				foreach (var yarnId in request.SelectedYarnIds)
				{
					await _unitOfWork.ProjectYarns.AddAsync(new ProjectYarn
					{
						ProjectId = id,
						YarnId = yarnId
					});
				}
			}

			await _unitOfWork.SaveChangesAsync();
			_logger.LogInformation("Project {Id} updated successfully with its yarns.", id);
		}

		public async Task DeleteAsync(int id)
		{
			var project = await _unitOfWork.Projects.GetByIdAsync(id);

			if (project == null)
				throw new Exception("Project not found");

			var allProjectYarns = await _unitOfWork.ProjectYarns.GetAllAsync();
			var yarnsToRemove = allProjectYarns.Where(py => py.ProjectId == id).ToList();

			foreach (var py in yarnsToRemove)
			{
				_unitOfWork.ProjectYarns.Remove(py);
			}

			_unitOfWork.Projects.Remove(project);

			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("Project {Id} and its yarn links deleted successfully.", id);
		}

		public async Task StartProjectAsync(int projectId)
		{
			var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
			if (project == null) throw new Exception("Project not found");

			project.Status = ProjectStatus.InProgress;
			project.StartDate = DateTime.UtcNow;

			_unitOfWork.Projects.Update(project);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task CompleteProjectAsync(int projectId)
		{
			var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
			if (project == null) throw new Exception("Project not found");

			project.Status = ProjectStatus.Completed;
			project.Progress = 100;

			_unitOfWork.Projects.Update(project);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task UpdateProgressAsync(int projectId, int progress)
		{
			if (progress < 0 || progress > 100)
				throw new Exception("Progress must be between 0 and 100");

			var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
			if (project == null) throw new Exception("Project not found");

			project.Progress = progress;

			if (progress == 100)
				project.Status = ProjectStatus.Completed;
			else if (progress > 0)
				project.Status = ProjectStatus.InProgress;

			_unitOfWork.Projects.Update(project);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<ProjectGetResponse> GetAsync(ProjectGetRequest request)
		{
			if (request.Pager == null)
			{
				request.Pager = new Amigurumemi.Contracts.Messaging.Requests.Shared.PagerRequest { Page = 1, PageSize = 6 };
			}

			int currentPage = request.Pager.Page <= 0 ? 1 : request.Pager.Page;
			int pageSize = request.Pager.PageSize <= 0 ? 6 : request.Pager.PageSize;

			var projects = await _unitOfWork.Projects.GetAllAsync();
			var query = projects.AsQueryable();
			var patterns = await _unitOfWork.Patterns.GetAllAsync();
			var yarns = await _unitOfWork.Yarns.GetAllAsync();

			var projectYarns = await _unitOfWork.ProjectYarns.GetAllAsync();

			if (request.Filter?.UserId != null && request.Filter.UserId > 0)
			{
				query = query.Where(x => x.UserId == request.Filter.UserId);
			}
			if (request.Filter != null && !string.IsNullOrWhiteSpace(request.Filter.Name))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(request.Filter.Name, StringComparison.OrdinalIgnoreCase));
			}
			if (request.Filter != null && request.Filter.Status != null)
			{
				query = query.Where(x => x.Status == request.Filter.Status.Value);
			}

			if (!string.IsNullOrWhiteSpace(request.OrderBy))
			{
				switch (request.OrderBy.ToLower())
				{
					case "name":
						query = request.SortAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
						break;
					case "progress":
						query = request.SortAsc ? query.OrderBy(x => x.Progress) : query.OrderByDescending(x => x.Progress);
						break;
					case "status":
						query = request.SortAsc ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.Status);
						break;
					case "startdate":
						query = request.SortAsc ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate);
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

			var pagedProjects = query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var pagedData = pagedProjects.Select(p => new ProjectResponse
			{
				Id = p.Id,
				Name = p.Name,
				Notes = p.Notes,
				Status = p.Status,
				Progress = p.Progress,
				StartDate = p.StartDate,
				FinishDate = p.FinishDate,
				EstimatedCost = p.EstimatedCost,
				IsFavorite = p.IsFavorite,
				UserId = p.UserId,
				PatternId = p.PatternId,
				PatternName = patterns.FirstOrDefault(x => x.Id == p.PatternId)?.Name ?? "Unknown Pattern",

				YarnNames = yarns
					.Where(y => projectYarns.Any(py => py.ProjectId == p.Id && py.YarnId == y.Id))
					.Select(y => y.Name)
					.ToList()
			}).ToList();

			return new ProjectGetResponse
			{
				Items = pagedData,
				Pager = new Amigurumemi.Contracts.Messaging.Responses.Shared.PagerResponse
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