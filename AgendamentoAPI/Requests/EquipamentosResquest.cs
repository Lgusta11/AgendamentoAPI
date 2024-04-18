using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.Requests
{
    public record EquipamentoRequest([Required] string Nome, [Required] int Quantidade);
}
