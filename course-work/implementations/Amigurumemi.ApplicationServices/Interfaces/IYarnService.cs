using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Amigurumemi.Contracts.Messaging.Responses.Yarns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IYarnService
	{
		Task CreateAsync(YarnRequest request);

		Task UpdateAsync(int id, YarnRequest request);

		Task DeleteAsync(int id);

		Task<YarnResponse?> GetByIdAsync(int id);

		Task<IEnumerable<YarnResponse>> GetAllAsync();

		Task<decimal> GetTotalCostByProjectAsync(int projectId);

		Task<IEnumerable<YarnResponse>> GetByColorAsync(string color);

		Task<YarnResponse?> GetMostExpensiveAsync();

		Task AssignToProjectAsync(int yarnId, int projectId);

		Task<YarnGetResponse> GetAsync(YarnGetRequest request);
	}
}
