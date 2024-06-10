using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaAfsWeb.Requests
{
    public record AgendamentoRequest([Required] DateTime Data, [Required] ICollection<int> AulaIds, [Required] int EquipamentoId, [Required] int ProfessorId);
}
