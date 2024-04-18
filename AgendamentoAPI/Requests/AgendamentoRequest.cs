using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.Requests
{
    public record AgendamentoRequest([Required] DateTime Data, [Required] int AulaId, [Required] int EquipamentoId, [Required] int ProfessorId);
}
