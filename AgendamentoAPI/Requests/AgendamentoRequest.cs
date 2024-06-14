using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.Requests
{
    public record AgendamentoRequest([Required] DateTime Data, [Required] ICollection<int> AulaIds, [Required] int EquipamentoId, [Required] string ProfessorId);
   
}
