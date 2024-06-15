using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Net.Http.Json;

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
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);

        return await _httpClient.GetFromJsonAsync<ICollection<EquipamentoResponse>>("equipamentos");
    }

    public async Task<EquipamentoResponse?> GetEquipamentoPorIdAsync(int id)
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);
        return await _httpClient.GetFromJsonAsync<EquipamentoResponse>($"equipamentos/{id}");
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
