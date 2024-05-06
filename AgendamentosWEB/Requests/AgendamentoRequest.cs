using System.ComponentModel.DataAnnotations;

namespace AgendamentosWEB.Requests
{
    public record AgendamentoRequest([Required] DateTime Data, [Required] int AulaId, [Required] int EquipamentoId, [Required] int ProfessorId);
}
