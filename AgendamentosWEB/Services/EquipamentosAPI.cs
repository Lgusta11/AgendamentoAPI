using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;

namespace AgendamentosWEB.Services;
public class EquipamentosAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public EquipamentosAPI(IHttpClientFactory factory, ILocalStorageService localStorageService)
    {
        _httpClient = factory.CreateClient("API");
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }

    public async Task<ICollection<EquipamentoResponse>?> GetEquipamentosAsync()
    {
        Console.WriteLine("Cheguei");
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);

        var request = await _httpClient.GetAsync("/equipamentos");

        if (!request.IsSuccessStatusCode) throw new Exception("Erro ao buscar equipamentos na api!");

        var response = await request.Content.ReadFromJsonAsync<ICollection<EquipamentoResponse>>();

        Console.WriteLine("Buscando...");

        return response;
    }

    public async Task<EquipamentoResponse?> GetEquipamentoPorIdAsync(int id)
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);

        var request = await _httpClient.GetAsync($"/equipamentos/{id}");

        if (!request.IsSuccessStatusCode) throw new Exception("Erro ao buscar equipamentos na api!");

        var response = await request.Content.ReadFromJsonAsync<EquipamentoResponse>();

        return response;
    }

    public async Task AddEquipamentoAsync(EquipamentoRequest equipamento)
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);
        await _httpClient.PostAsJsonAsync("equipamentos", equipamento);
    }

    public async Task DeleteEquipamentoAsync(int id)
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);
        await _httpClient.DeleteAsync($"equipamentos/{id}");
    }

    public async Task UpdateEquipamentoAsync(EquipamentosRequestEdit equipamento)
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);
        await _httpClient.PutAsJsonAsync($"equipamentos/{equipamento.Id}", equipamento);
    }
}
