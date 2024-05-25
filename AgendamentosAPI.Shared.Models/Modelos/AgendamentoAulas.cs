using Agendamentos.Shared.Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentosAPI.Shared.Models.Modelos
{
    public class AgendamentoAula
    {
        public int AgendamentoId { get; set; }
        public virtual Agendamento Agendamento { get; set; }

        public int AulaId { get; set; }
        public virtual Aulas Aula { get; set; }
    }
}
