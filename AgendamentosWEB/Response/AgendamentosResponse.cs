using AgendamentosWEB.Response;

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