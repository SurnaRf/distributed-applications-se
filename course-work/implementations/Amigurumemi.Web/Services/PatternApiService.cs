using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Patterns;

public class PatternApiService
{
	private readonly HttpClient _httpClient;

	public PatternApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("Api");
	}

	public async Task<List<PatternResponse>> GetAllAsync()
	{
		return await _httpClient
			.GetFromJsonAsync<List<PatternResponse>>
			("api/Patterns");
	}

	public async Task<PatternResponse?> GetByIdAsync(int id)
	{
		return await _httpClient
			.GetFromJsonAsync<PatternResponse>($"api/Patterns/{id}");
	}

	public async Task CreateAsync(PatternRequest request)
	{
		await _httpClient.PostAsJsonAsync("api/Patterns", request);
	}

	public async Task UpdateAsync(int id, PatternRequest request)
	{
		await _httpClient.PutAsJsonAsync($"api/Patterns/{id}", request);
	}

	public async Task DeleteAsync(int id)
	{
		var response = await _httpClient.DeleteAsync($"api/Patterns/{id}");

		if (!response.IsSuccessStatusCode)
		{
			var errorResult = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
			if (errorResult != null && errorResult.ContainsKey("error"))
			{
				throw new Exception(errorResult["error"]);
			}

			throw new Exception("An error occurred while trying to delete the pattern.");
		}
	}

	public async Task<PatternGetResponse?> SearchAsync(PatternGetRequest request)
	{
		return await _httpClient.PostAsJsonAsync(
			"api/Patterns/search",
			request)
			.Result
			.Content
			.ReadFromJsonAsync<PatternGetResponse>();
	}
}