namespace AgendamentosWEB.Services;
using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Net.Http.Json;

public class AulasAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public AulasAPI(IHttpClientFactory factory, ILocalStorageService localStorageService)
    {
        _httpClient = factory.CreateClient("API");
        _localStorageService = localStorageService;
    }

    public async Task<ICollection<AulasResponse>?> GetAulasAsync()
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        return await _httpClient.GetFromJsonAsync<ICollection<AulasResponse>>("aulas");
    }

    public async Task AddAulaAsync(AulasRequest aula)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        await _httpClient.PostAsJsonAsync("aulas", aula);
    }

    public async Task DeleteAulaAsync(int id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);
        await _httpClient.DeleteAsync($"aulas/{id}");
    }

    public async Task<AulasResponse?> GetAulaPorIdAsync(int id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);
        return await _httpClient.GetFromJsonAsync<AulasResponse>($"aulas/{id}");
    }

    public async Task UpdateAulaAsync(AulasRequestEdit aula)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);
        await _httpClient.PutAsJsonAsync($"aulas", aula);
    }
}

