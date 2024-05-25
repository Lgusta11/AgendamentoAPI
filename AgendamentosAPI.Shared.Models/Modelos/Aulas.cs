using AgendamentosAPI.Shared.Models.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Aulas
    {
        public Aulas(string aula)
        {
            Aula = aula;
        }

        public int Id { get; set; }
        public string? Aula { get; set; } // "Primeira Aula", "Segunda Aula", etc.
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public TimeSpan Duracao { get; set; }
        public virtual ICollection<AgendamentoAula> AgendamentoAulas { get; set; }

    }

}
