using System.ComponentModel.DataAnnotations;

namespace SistemaAfsWeb.Requests
{
    public record ProfessoresRequest
    {
        [Required]
        public string? nome { get; set; }

        [Required, EmailAddress]
        public string? email { get; set; }


        [Required, MinLength(6)]
        public string? senha { get; set; }

        [Required]
        public string? confirmacaoSenha { get; set; }


        public ProfessoresRequest() { }

        public ProfessoresRequest(string confirmacaoSenha, string nome, string senha, string email)
        {

            nome = nome;
            senha = senha;
            email = email;
            confirmacaoSenha = confirmacaoSenha;

        }

    }
}
