using AgendamentosWEB.Requests;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class LoginAPI
{
    private readonly HttpClient _httpClient;
    public LoginAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<string?> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Login", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            if (content != null && content.ContainsKey("token"))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content["token"]);
                return content["token"];
            }
        }
        return null;
    }

}