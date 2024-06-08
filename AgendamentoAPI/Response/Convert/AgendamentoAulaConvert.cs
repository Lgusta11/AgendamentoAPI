using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;

namespace AgendamentoAPI.Response.Convert
{
    public static class AgendamentoAulaConvert
    {
        public static AgendamentoAulaResponse ToResponse(this AgendamentoAula agendamento)
        {
            return new AgendamentoAulaResponse
            {
                Id = agendamento.AulaId,
                Aula = agendamento.Aula.Aula,
                Data = agendamento.Agendamento.Data
            };
        }
    }
}