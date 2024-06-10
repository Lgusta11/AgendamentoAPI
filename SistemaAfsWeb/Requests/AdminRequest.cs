using System.ComponentModel.DataAnnotations;

namespace SistemaAfsWeb.Requests
{
    public record AdminRequest([Required] string Nome, [Required, EmailAddress] string Email, [Required, MinLength(6)] string Senha, [Required] string ConfirmacaoSenha);

}
