using System.ComponentModel.DataAnnotations;

namespace AgendamentosWEB.Requests
{
    public record EquipamentoRequest([Required] string Nome, [Required] int Quantidade);
}
