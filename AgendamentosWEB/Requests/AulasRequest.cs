using System.ComponentModel.DataAnnotations;

namespace AgendamentosWEB.Requests
{
    public record AulasRequest([Required] string Aula, TimeSpan Duracao);
}
