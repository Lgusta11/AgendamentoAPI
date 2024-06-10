using SistemaAfsWeb.Requests;
using SistemaAfsWeb.Response;
using System.Net.Http.Json;

namespace SistemaAfsWeb.Services;

public class AdminAPI
{
    private readonly HttpClient _httpClient;
    public AdminAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<AdminResponse>?> GetAdminsAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AdminResponse>>("Admin");
    }

    public async Task<AdminResponse?> GetAdminPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AdminResponse>($"Admin/{id}");
    }

    public async Task UpdateAdminAsync(AdminRequestEdit admin)
    {
        await _httpClient.PutAsJsonAsync($"Admin/{admin.Id}", admin);
    }

    public async Task DeleteAdminAsync(int id)
    {
        await _httpClient.DeleteAsync($"Admin/{id}");
    }
}