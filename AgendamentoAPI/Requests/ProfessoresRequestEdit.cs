namespace Agendamentos.Requests
{
    public record ProfessoresRequestEdit(int Id, string nome, string email, string senha, string confirmacaoSenha)
        : ProfessoresRequest(nome, email, senha, confirmacaoSenha);
}
