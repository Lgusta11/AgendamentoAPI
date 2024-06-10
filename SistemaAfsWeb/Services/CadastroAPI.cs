using SistemaAfsWeb.Requests;
using SistemaAfsWeb.Response;
using System.Net.Http.Json;

namespace SistemaAfsWeb.Services;
public class CadastroAPI
{
    private readonly HttpClient _httpClient;
    public CadastroAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<AdminResponse?> CadastroAdminAsync(AdminRequest admin)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Cadastro/admin", admin);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AdminResponse>();
        }
        return null;
    }

    public async Task<ProfessoresResponse?> CadastroProfessorAsync(ProfessoresRequest professor)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Cadastro/Professor", professor);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ProfessoresResponse>();
        }
        return null;
    }
}

