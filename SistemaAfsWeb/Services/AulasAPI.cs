namespace SistemaAfsWeb.Services;
using SistemaAfsWeb.Requests;
using SistemaAfsWeb.Response;
using System.Net.Http.Json;

public class AulasAPI
{
    private readonly HttpClient _httpClient;
    public AulasAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<AulasResponse>?> GetAulasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AulasResponse>>("aulas");
    }

    public async Task AddAulaAsync(AulasRequest aula)
    {
        await _httpClient.PostAsJsonAsync("aulas", aula);
    }

    public async Task DeleteAulaAsync(int id)
    {
        await _httpClient.DeleteAsync($"aulas/{id}");
    }

    public async Task<AulasResponse?> GetAulaPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AulasResponse>($"aulas/{id}");
    }

    public async Task UpdateAulaAsync(AulasRequestEdit aula)
    {
        await _httpClient.PutAsJsonAsync($"aulas", aula);
    }
}

