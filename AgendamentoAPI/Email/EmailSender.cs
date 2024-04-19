using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity;

namespace AgendamentoAPI.Email
{
    public class DummyEmailSender : IEmailSender<PessoaComAcesso>
    {
        public Task SendConfirmationLinkAsync(PessoaComAcesso user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(PessoaComAcesso user, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(PessoaComAcesso user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(PessoaComAcesso user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
