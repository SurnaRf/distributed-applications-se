using Amigurumemi.Contracts.Messaging.Requests.Auth;
using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Auth;
using System.Net.Http.Json;

public class AuthApiService
{
	private readonly HttpClient _httpClient;

	public AuthApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("Api");
	}

	public async Task<AuthTokenResponse?> LoginAsync(AuthTokenRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);

		if (!response.IsSuccessStatusCode)
			return null;

		return await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
	}

	public async Task<bool> RegisterAsync(UserRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Users", request);
		return response.IsSuccessStatusCode;
	}

	public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Auth/change-password", request);
		return response.IsSuccessStatusCode;
	}
}