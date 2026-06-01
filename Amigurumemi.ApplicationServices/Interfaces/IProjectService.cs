using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IProjectService
	{
		Task CreateAsync(ProjectRequest request);

		Task UpdateAsync(int id, ProjectRequest request);

		Task DeleteAsync(int id);

		Task<ProjectResponse?> GetByIdAsync(int id);

		Task<IEnumerable<ProjectResponse>> GetAllAsync();

		Task StartProjectAsync(int projectId);

		Task CompleteProjectAsync(int projectId);

		Task UpdateProgressAsync(int projectId, int progress);

		Task<ProjectGetResponse> GetAsync(ProjectGetRequest request);
	}
}
