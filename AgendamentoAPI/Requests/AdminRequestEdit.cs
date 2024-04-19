using Agendamentos.Requests;

namespace AgendamentoAPI.Requests
{
     public record AdminRequestEdit(int Id, string Nome, string Email, string Senha, string ConfirmacaoSenha)
       : AdminRequest(Nome, Email, Senha, ConfirmacaoSenha);
}
