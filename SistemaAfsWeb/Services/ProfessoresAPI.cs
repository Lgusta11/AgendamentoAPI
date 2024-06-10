using SistemaAfsWeb.Requests;
using SistemaAfsWeb.Response;
using System.Net.Http.Json;

namespace SistemaAfsWeb.Services;
public class ProfessoresAPI
{
    private readonly HttpClient _httpClient;
    public ProfessoresAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<ProfessoresResponse>?> GetProfessoresAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ProfessoresResponse>>("professores");
    }

    public async Task<ProfessoresResponse?> GetProfessorPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProfessoresResponse>($"professores/{id}");
    }


    public async Task DeleteProfessorAsync(int id)
    {
        await _httpClient.DeleteAsync($"professores/{id}");
    }

    public async Task UpdateProfessorAsync(ProfessoresRequestEdit professor)
    {
        await _httpClient.PutAsJsonAsync($"professores/{professor.Id}", professor);
    }

    public async Task<int?> GetProfessorIdByUserNameAsync(string userName)
    {
        return await _httpClient.GetFromJsonAsync<int?>($"professores/getProfessorId/{userName}");
    }
}