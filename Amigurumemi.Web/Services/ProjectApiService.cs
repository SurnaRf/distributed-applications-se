using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Projects;
using System.Net.Http.Json;

public class ProjectApiService
{
	private readonly HttpClient _httpClient;

	public ProjectApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("Api");
	}

	public async Task<List<ProjectResponse>> GetAllAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<ProjectResponse>>("api/Projects")
			   ?? new List<ProjectResponse>();
	}

	public async Task<ProjectResponse?> GetByIdAsync(int id)
	{
		return await _httpClient.GetFromJsonAsync<ProjectResponse>($"api/Projects/{id}");
	}

	public async Task CreateAsync(ProjectRequest request)
	{
		await _httpClient.PostAsJsonAsync("api/Projects", request);
	}

	public async Task UpdateAsync(int id, ProjectRequest request)
	{
		await _httpClient.PutAsJsonAsync($"api/Projects/{id}", request);
	}

	public async Task DeleteAsync(int id)
	{
		await _httpClient.DeleteAsync($"api/Projects/{id}");
	}

	public async Task StartProjectAsync(int id)
	{
		await _httpClient.PostAsync($"api/Projects/{id}/start", null);
	}

	public async Task CompleteProjectAsync(int id)
	{
		await _httpClient.PostAsync($"api/Projects/{id}/complete", null);
	}

	public async Task UpdateProgressAsync(int id, int progress)
	{
		await _httpClient.PatchAsync($"api/Projects/{id}/progress?progress={progress}", null);
	}

	public async Task<ProjectGetResponse?> SearchAsync(ProjectGetRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Projects/search", request);

		if (!response.IsSuccessStatusCode)
		{
			return new ProjectGetResponse
			{
				Items = new List<ProjectResponse>()
			};
		}
		return await response.Content.ReadFromJsonAsync<ProjectGetResponse>();
	}


}