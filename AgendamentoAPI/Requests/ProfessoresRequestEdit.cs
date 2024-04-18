namespace Agendamentos.Requests
{
    public record ProfessoresRequestEdit(int Id, string nome, string? fotoPerfil, string email, string senha, string confirmacaoSenha)
        : ProfessoresRequest(nome, fotoPerfil, email, senha, confirmacaoSenha);
}
