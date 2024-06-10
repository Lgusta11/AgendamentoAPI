using System.ComponentModel.DataAnnotations;

namespace SistemaAfsWeb.Requests
{
    public record EquipamentoRequest([Required] string Nome, [Required] int Quantidade);
}
