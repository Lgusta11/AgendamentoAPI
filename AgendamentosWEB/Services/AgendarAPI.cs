using AgendamentoAPI.Response;
using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class AgendamentosAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AgendamentosAPI> _logger;
    private readonly ILocalStorageService _localStorageService;

    public AgendamentosAPI(IHttpClientFactory factory, ILogger<AgendamentosAPI> logger, ILocalStorageService localStorage)
    {
        _httpClient = factory.CreateClient("API");
        _logger = logger;
        _localStorageService = localStorage;

    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosAsync()
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);


        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>("agendamentos");
    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosPorProfessorIdAsync(int id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>($"agendamentos/{id}");
    }

    public async Task<HttpResponseMessage> AddAgendamentoAsync(AgendamentoRequest agendamento)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        try
        {
            return await _httpClient.PostAsJsonAsync("agendamentos", agendamento);
        }
        catch (Exception ex)
        {
           
            throw new Exception("Erro ao adicionar agendamento", ex);
        }
    }
    public async Task DeleteAgendamentoAsync(int id)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        await _httpClient.DeleteAsync($"agendamentos/{id}");
    }

    public async Task UpdateAgendamentoAsync(AgendamentosRequestEdit agendamento)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        await _httpClient.PutAsJsonAsync($"agendamentos/{agendamento.Id}", agendamento);
    }

   

}