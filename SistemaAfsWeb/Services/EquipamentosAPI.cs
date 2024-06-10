using SistemaAfsWeb.Requests;
using SistemaAfsWeb.Response;
using System.Net.Http.Json;


namespace SistemaAfsWeb.Services;
public class EquipamentosAPI
{
    private readonly HttpClient _httpClient;
    public EquipamentosAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<EquipamentoResponse>?> GetEquipamentosAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<EquipamentoResponse>>("equipamentos");
    }

    public async Task<EquipamentoResponse?> GetEquipamentoPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<EquipamentoResponse>($"equipamentos/{id}");
    }

    public async Task AddEquipamentoAsync(EquipamentoRequest equipamento)
    {
        await _httpClient.PostAsJsonAsync("equipamentos", equipamento);
    }

    public async Task DeleteEquipamentoAsync(int id)
    {
        await _httpClient.DeleteAsync($"equipamentos/{id}");
    }

    public async Task UpdateEquipamentoAsync(EquipamentosRequestEdit equipamento)
    {
        await _httpClient.PutAsJsonAsync($"equipamentos/{equipamento.Id}", equipamento);
    }
}

