using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Contracts.Messaging.Requests.Shared;
using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Contracts.Messaging.Responses.Users;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class UserService : BaseService, IUserService
	{
		private readonly IPasswordService _passwordService;

		public UserService(
			ILogger<UserService> logger,
			IUnitOfWork unitOfWork,
			IPasswordService passwordService)
			: base(logger, unitOfWork)
		{
			_passwordService = passwordService;
		}

		public async Task CreateAsync(UserRequest request)
		{
			try
			{
				var user = new User
				{
					Username = request.Username,
					Email = request.Email,
					FirstName = request.FirstName,
					LastName = request.LastName,
					RegistrationDate = DateTime.UtcNow,
					Role = "User"
				};

				user.PasswordHash = _passwordService.HashPassword(user, request.Password);

				await _unitOfWork.Users.AddAsync(user);
				await _unitOfWork.SaveChangesAsync();

				_logger.LogInformation( "User {Username} created successfully.", request.Username);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,"Error creating user {Username}", request.Username);

				throw;
			}
		}

		public async Task<UserResponse?> GetByIdAsync(int id)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);

			if (user == null)
			{
				_logger.LogWarning("User {Id} not found.",id);

				return null;
			}

			return new UserResponse
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role,
				RegistrationDate = user.RegistrationDate
			};
		}

		public async Task<IEnumerable<UserResponse>> GetAllAsync()
		{
			var users = await _unitOfWork.Users.GetAllAsync();

			return users.Select(u => new UserResponse
			{
				Id = u.Id,
				Username = u.Username,
				Email = u.Email,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Role = u.Role,
				RegistrationDate = u.RegistrationDate
			});
		}

		public async Task UpdateAsync(int id, UserRequest request)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);

			if (user == null)
				throw new Exception("User not found");

			user.Username = request.Username;
			user.Email = request.Email;
			user.FirstName = request.FirstName;
			user.LastName = request.LastName;

			user.PasswordHash = _passwordService.HashPassword(user, request.Password);

			_unitOfWork.Users.Update(user);

			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("User {Id} updated successfully.", id);
		}

		public async Task DeleteAsync(int id)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);

			if (user == null)
				throw new Exception("User not found");

			_unitOfWork.Users.Remove(user);

			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("User {Id} deleted successfully.", id);
		}

		public async Task<UserStatsResponse> GetUserStatsAsync(int userId)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(userId);

			if (user == null)
				throw new Exception("User not found");

			var projects =
				await _unitOfWork.Projects.FindAsync(p => p.UserId == userId);

			var patterns =
				await _unitOfWork.Patterns.FindAsync(p => p.UserId == userId);

			return new UserStatsResponse
			{
				UserId = userId,
				TotalProjects = projects.Count(),
				ActiveProjects = projects.Count(p => p.Status != ProjectStatus.Completed),
				CompletedProjects = projects.Count(p => p.Status == ProjectStatus.Completed),
				PatternsCreated = patterns.Count()
			};
		}

		public async Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId)
		{
			var projects = await _unitOfWork.Projects.FindAsync(p => p.UserId == userId);

			return projects.Select(p => new ProjectResponse
			{
				Id = p.Id,
				Name = p.Name,
				Status = p.Status,
				Progress = p.Progress,
				StartDate = p.StartDate,
				PatternId = p.PatternId,
				UserId = p.UserId
			});
		}

		public async Task ChangePasswordAsync(
			int userId,
			string oldPassword,
			string newPassword)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(userId);

			if (user == null)
				throw new Exception("User not found");

			var isValid = _passwordService.VerifyPassword(user, user.PasswordHash, oldPassword);

			if (!isValid)
				throw new Exception("Old password is incorrect");

			user.PasswordHash = _passwordService.HashPassword(user, newPassword);

			_unitOfWork.Users.Update(user);

			await _unitOfWork.SaveChangesAsync();
		}

		public async Task AssignRoleAsync(int userId, string role)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(userId);

			if (user == null)
				throw new Exception("User not found");

			user.Role = role;

			_unitOfWork.Users.Update(user);

			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<UsersGetResponse> GetAsync(UserGetRequest request)
		{
			if (request.Pager == null)
			{
				request.Pager = new PagerRequest { Page = 1, PageSize = 5 };
			}

			int currentPage = request.Pager.Page <= 0 ? 1 : request.Pager.Page;
			int pageSize = request.Pager.PageSize <= 0 ? 5 : request.Pager.PageSize;

			var users = await _unitOfWork.Users.GetAllAsync();
			var query = users.AsQueryable();

			if (request.Filter != null && !string.IsNullOrWhiteSpace(request.Filter.Username))
			{
				query = query.Where(x => x.Username != null &&
										 x.Username.Contains(request.Filter.Username, StringComparison.OrdinalIgnoreCase));
			}

			if (request.Filter != null && !string.IsNullOrWhiteSpace(request.Filter.Email))
			{
				query = query.Where(x => x.Email != null &&
										 x.Email.Contains(request.Filter.Email, StringComparison.OrdinalIgnoreCase));
			}

			var totalCount = query.Count();

			var pagedData = query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.Select(u => new UserResponse
				{
					Id = u.Id,
					Username = u.Username,
					Email = u.Email,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Role = u.Role,
					RegistrationDate = u.RegistrationDate
				})
				.ToList();

			return new UsersGetResponse
			{
				Items = pagedData,
				Pager = new PagerResponse
				{
					Page = currentPage,
					PageSize = pageSize,
					Count = totalCount
				},
				Filter = request.Filter
			};
		}
	}
}