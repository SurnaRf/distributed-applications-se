using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.ApplicationServices.Interfaces
{
	public interface IPatternService
	{
		Task CreateAsync(PatternRequest request);

		Task UpdateAsync(int id, PatternRequest request);

		Task DeleteAsync(int id);

		Task<PatternResponse?> GetByIdAsync(int id);

		Task<IEnumerable<PatternResponse>> GetAllAsync();

		Task<PatternGetResponse> GetAsync(PatternGetRequest request);
	}
}
