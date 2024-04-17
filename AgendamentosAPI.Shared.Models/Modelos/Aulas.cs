using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Aulas
    {
        public int Id { get; set; }
        public string? Aula { get; set; } // "Primeira Aula", "Segunda Aula", etc.
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    }

}
