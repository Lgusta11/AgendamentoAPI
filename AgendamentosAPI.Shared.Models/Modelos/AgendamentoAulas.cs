using Agendamentos.Shared.Modelos.Modelos;

namespace AgendamentosAPI.Shared.Models.Modelos
{
    public class AgendamentoAula
    {
        public int AgendamentoId { get; set; }
        public virtual Agendamento Agendamento { get; set; } = null!;

        public int AulaId { get; set; }
        public virtual Aulas Aula { get; set; } = null!;
    }
}
