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

        await _httpClient.DeleteAsync($"users/{id}");
    }

    public async Task UpdateProfessorAsync(UserRequestEdit user)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);


        await _httpClient.PutAsJsonAsync($"users/{user.Id}", user);
    }

    
    }
