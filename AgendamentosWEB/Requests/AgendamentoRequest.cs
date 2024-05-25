using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AgendamentosWEB.Requests
{
    public record AgendamentoRequest([Required] DateTime Data, [Required] ICollection<int> AulaIds, [Required] int EquipamentoId, [Required] int ProfessorId);
}
