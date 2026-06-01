using System.Net.Http.Json;
using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Amigurumemi.Contracts.Messaging.Responses.Yarns;

public class YarnApiService
{
	private readonly HttpClient _httpClient;

	public YarnApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("Api");
	}

	public async Task<List<YarnResponse>> GetAllAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<YarnResponse>>("api/Yarns")
			   ?? new List<YarnResponse>();
	}

	public async Task<YarnResponse?> GetByIdAsync(int id)
	{
		return await _httpClient.GetFromJsonAsync<YarnResponse>($"api/Yarns/{id}");
	}

	public async Task CreateAsync(YarnRequest request)
	{
		await _httpClient.PostAsJsonAsync("api/Yarns", request);
	}

	public async Task UpdateAsync(int id, YarnRequest request)
	{
		await _httpClient.PutAsJsonAsync($"api/Yarns/{id}", request);
	}

	public async Task DeleteAsync(int id)
	{
		await _httpClient.DeleteAsync($"api/Yarns/{id}");
	}

	public async Task AssignToProjectAsync(int yarnId, int projectId)
	{
		await _httpClient.PostAsync($"api/Yarns/{yarnId}/assign/{projectId}", null);
	}
	public async Task<YarnGetResponse?> SearchAsync(YarnGetRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Yarns/search", request);

		if (!response.IsSuccessStatusCode)
		{
			return new YarnGetResponse
			{
				Items = new List<YarnResponse>()
			};
		}
		return await response.Content.ReadFromJsonAsync<YarnGetResponse>();
	}
}