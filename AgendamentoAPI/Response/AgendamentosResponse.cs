using Agendamentos.Response;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;

namespace AgendamentoAPI.Response
{
    public record AgendamentoResponse(int Id, DateTime Data, string NomeAula, string NomeEquipamento, string ProfessorNome);
}
