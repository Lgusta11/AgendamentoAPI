using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class AgendamentosAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AgendamentosAPI> _logger;
    public AgendamentosAPI(IHttpClientFactory factory, ILogger<AgendamentosAPI> logger)
    {
        _httpClient = factory.CreateClient("API");
        _logger = logger;
    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>("agendamentos");
    }

    public async Task<ICollection<AgendamentoResponse>?> GetAgendamentosPorProfessorIdAsync(int professorId)
    {
        return await _httpClient.GetFromJsonAsync<ICollection<AgendamentoResponse>>($"agendamentos/{professorId}");
    }

    public async Task<HttpResponseMessage> AddAgendamentoAsync(AgendamentoRequest agendamento)
    {
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
        await _httpClient.DeleteAsync($"agendamentos/{id}");
    }

    public async Task UpdateAgendamentoAsync(AgendamentosRequestEdit agendamento)
    {
        await _httpClient.PutAsJsonAsync($"agendamentos/{agendamento.Id}", agendamento);
    }

    public async Task<string> GetNomeAulaAsync(int aulaId)
    {
        var response = await _httpClient.GetAsync($"aulas/{aulaId}");

        if (response.IsSuccessStatusCode)
        {
            var aula = await response.Content.ReadFromJsonAsync<AulasResponse>();
            return aula.Aula;
        }

        _logger.LogWarning("Não foi possível recuperar o nome da aula {AulaId}", aulaId);
        return null;
    }

    public async Task<string> GetNomeEquipamentoAsync(int equipamentoId)
    {
        var response = await _httpClient.GetAsync($"equipamentos/{equipamentoId}");

        if (response.IsSuccessStatusCode)
        {
            var equipamento = await response.Content.ReadFromJsonAsync<EquipamentoResponse>();
            return equipamento.Nome;
        }

        _logger.LogWarning("Não foi possível recuperar o nome do equipamento {EquipamentoId}", equipamentoId);
        return null;
    }

    public async Task<InfoPessoaResponse> GetUserInfoAsync()
    {
        var response = await _httpClient.GetAsync("auth/manage/info");
        return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
    }

}