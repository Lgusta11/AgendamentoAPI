using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class AgendamentosAPI
{
    private readonly HttpClient _httpClient;
    public AgendamentosAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>("agendamentos");
    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosPorProfessorIdAsync(int professorId)
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>($"agendamentos/{professorId}");
    }

    public async Task AddAgendamentoAsync(AgendamentoRequest agendamento)
    {
        await _httpClient.PostAsJsonAsync("agendamentos", agendamento);
    }

    public async Task DeleteAgendamentoAsync(int id)
    {
        await _httpClient.DeleteAsync($"agendamentos/{id}");
    }

    public async Task UpdateAgendamentoAsync(AgendamentosRequestEdit agendamento)
    {
        await _httpClient.PutAsJsonAsync($"agendamentos/{agendamento.Id}", agendamento);
    }
}