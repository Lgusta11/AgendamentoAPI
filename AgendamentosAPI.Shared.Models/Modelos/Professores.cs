using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Professores
    {
        public string? email;

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();


        public Professores(string nome)
        {
            Nome = nome;
            

        }

        public string Nome { get; set; }
        public int Id { get; set; }
        public string? Email { get; set; }
        public string UserId { get; set; }
        public void AdicionarAgendamento(Agendamento agendamento)
        {
            
            if (Agendamentos.Any(a => a.Data.Date == agendamento.Data.Date && a.AulaId == agendamento.AulaId && a.EquipamentoId == agendamento.EquipamentoId && a.ProfessorId == this.Id))
            {
                throw new InvalidOperationException("Você já criou um agendamento para este dia, aula e equipamento.");
            }

            Agendamentos.Add(agendamento);
        }

        public override string ToString()
        {
            return $@"Id: {Id}
            Nome: {Nome}";
            



        }
    }
}
