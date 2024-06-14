using AgendamentosAPI.Shared.Models.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Agendamento
    {

        public Agendamento()
        {
            AgendamentoAulas = new List<AgendamentoAula>();
        }

        public int Id { get; set; }

        public DateTime Data { get; set; }

        public int EquipamentoId { get; set; }

        public virtual Equipamentos? Equipamento { get; set; }

        public string ProfessorId { get; set; }

        public virtual User? Professor { get; set; }

        public virtual ICollection<AgendamentoAula>? AgendamentoAulas { get; set; }

    }

}
