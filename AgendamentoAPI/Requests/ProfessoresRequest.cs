using System.ComponentModel.DataAnnotations;

namespace Agendamentos.Requests
{
    public record ProfessoresRequest([Required] string nome, [Required, EmailAddress] string email, [Required, MinLength(6)] string senha, [Required] string confirmacaoSenha);

}
