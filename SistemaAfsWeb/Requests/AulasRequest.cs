using System.ComponentModel.DataAnnotations;

namespace SistemaAfsWeb.Requests
{
    public record AulasRequest([Required] string Aula, TimeSpan Duracao);
}
