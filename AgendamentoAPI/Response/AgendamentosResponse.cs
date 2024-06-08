using Agendamentos.Response;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;

namespace AgendamentoAPI.Response
{
    public class AgendamentoResponse
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public virtual EquipamentoResponse? Equipamento { get; set; }
        public virtual ProfessoresResponse? Professor { get; set; }
        public virtual ICollection<AgendamentoAulaResponse>? Aulas { get; set; }
    }
}