using System.ComponentModel.DataAnnotations;

namespace AgendamentosWEB.Requests
{
    public record ProfessoresRequest {
        [Required]
        public string? Nome { get; set; }

        [Required, EmailAddress] 
        public string? Email { get; set; }


            [Required, MinLength(6)] 
            public string? Senha { get; set; }

            [Required] 
            public string? ConfirmacaoSenha { get; set; }


        public ProfessoresRequest() { }

        public ProfessoresRequest(string confirmacaoSenha, string nome, string senha, string email)
        {
    
            Nome = nome;
            Senha = senha;
            Email = email;
            ConfirmacaoSenha = confirmacaoSenha;

        }

    }
}
