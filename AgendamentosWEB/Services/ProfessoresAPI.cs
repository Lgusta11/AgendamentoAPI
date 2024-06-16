using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class ProfessoresAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    public ProfessoresAPI(IHttpClientFactory factory, ILocalStorageService localStorageService)
    {
        _httpClient = factory.CreateClient("API");
        _localStorageService = localStorageService;
    }

    public async Task<ICollection<ProfessoresResponse>?> GetProfessoresAsync()
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        var request = await _httpClient.GetAsync("/professores");

        if (!request.IsSuccessStatusCode) throw new Exception("Erro ao listar professores da API!");

        var response = await request.Content.ReadFromJsonAsync<ICollection<ProfessoresResponse>>();

        return response;
    }

    public async Task<ProfessoresResponse?> GetProfessorPorIdAsync(string id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        var request = await _httpClient.GetAsync($"/professores/{id}");

        if (!request.IsSuccessStatusCode) throw new Exception("Erro ao listar professores da API!");

        var response = await request.Content.ReadFromJsonAsync<ProfessoresResponse>();

        return response;
    }


    public async Task DeleteProfessorAsync(string id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        await _httpClient.DeleteAsync($"professores/{id}");
    }

    public async Task UpdateProfessorAsync(ProfessoresRequestEdit professor)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        await _httpClient.PutAsJsonAsync($"professores/{professor.Id}", professor);
    }

    public async Task<string> GetProfessorIdByUserNameAsync(string userName)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        return await _httpClient.GetFromJsonAsync<string?>($"professores/id/{userName}");
    }
}