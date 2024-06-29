using AgendamentosAPI.Shared.Models.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Agendamento
    {
        public Agendamento()
        {
            AgendamentoAulas = new List<AgendamentoAula>();
        }

        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int EquipamentoId { get; set; }
        public virtual Equipamentos Equipamento { get; set; } = null!;
        public int AulaId { get; set; }
        public string ProfessorId { get; set; } = null!;
        public virtual User Professor { get; set; } = null!;

        public virtual ICollection<AgendamentoAula> AgendamentoAulas { get; set; } = null!;
    }
}
