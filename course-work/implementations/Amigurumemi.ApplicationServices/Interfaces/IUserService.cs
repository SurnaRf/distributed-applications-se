using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IUserService
	{
		Task CreateAsync(UserRequest request);

		Task UpdateAsync(int id, UserRequest request);

		Task DeleteAsync(int id);

		Task<UserResponse?> GetByIdAsync(int id);

		Task<IEnumerable<UserResponse>> GetAllAsync();

		Task<UserStatsResponse> GetUserStatsAsync(int userId);

		Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId);

		Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);

		Task AssignRoleAsync(int userId, string role);

		Task<UsersGetResponse> GetAsync(UserGetRequest request);
	}
}
