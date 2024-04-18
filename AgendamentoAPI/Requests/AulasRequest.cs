using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.Requests
{
    public record AulasRequest([Required] string Aula, TimeSpan Duracao);
}
