using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Agendamento
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int AulaId { get; set; }
        public virtual Aulas? Aula { get; set; }
        public int EquipamentoId { get; set; }
        public virtual Equipamentos? Equipamento { get; set; }
        public int ProfessorId { get; set; }
        public virtual Professores? Professor { get; set; }

    }

}
