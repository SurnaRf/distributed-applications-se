using System.Net.Http.Json;
using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Users;

public class UserApiService
{
	private readonly HttpClient _httpClient;

	public UserApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("Api");
	}

	public async Task<List<UserResponse>> GetAllAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/Users")
			   ?? new List<UserResponse>();
	}

	public async Task<UserResponse?> GetByIdAsync(int id)
	{
		return await _httpClient.GetFromJsonAsync<UserResponse>($"api/Users/{id}");
	}

	public async Task UpdateAsync(int id, UserRequest request)
	{
		await _httpClient.PutAsJsonAsync($"api/Users/{id}", request);
	}

	public async Task DeleteAsync(int id)
	{
		await _httpClient.DeleteAsync($"api/Users/{id}");
	}

	public async Task<UserStatsResponse?> GetUserStatsAsync(int id)
	{
		return await _httpClient.GetFromJsonAsync<UserStatsResponse>($"api/Users/{id}/stats");
	}

	public async Task<UsersGetResponse?> SearchAsync(UserGetRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Users/search", request);

		if (!response.IsSuccessStatusCode)
		{
			return new UsersGetResponse
			{
				Items = new List<UserResponse>()
			};
		}
		return await response.Content.ReadFromJsonAsync<UsersGetResponse>();
	}
}