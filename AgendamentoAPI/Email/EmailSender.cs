using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;
using Agendamentos.Shared.Modelos.Modelos;

namespace AgendamentoAPI.Email
{
    public class DummyEmailSender : IEmailSender<PessoaComAcesso>
    {
        public async Task SendConfirmationLinkAsync(PessoaComAcesso user, string email, string confirmationLink)
        {
            string subject = "Confirmação de Conta";
            string htmlMessage = $"Por favor, confirme sua conta clicando neste link: {confirmationLink}";
            await SendEmailAsync(user, subject, htmlMessage);
        }

        public async Task SendPasswordResetCodeAsync(PessoaComAcesso user, string email, string resetCode)
        {
            string subject = "Redefinição de Senha";
            string htmlMessage = $"Seu código de redefinição de senha é: {resetCode}";
            await SendEmailAsync(user, subject, htmlMessage);
        }
        public async Task SendPasswordResetLinkAsync(PessoaComAcesso user, string email, string resetLink)
        {
            string subject = "Redefinição de Senha";
            string htmlMessage = $"Por favor, redefina sua senha clicando neste link: {resetLink}";
            await SendEmailAsync(user, subject, htmlMessage);
        }

        private async Task SendEmailAsync(PessoaComAcesso user, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Agendamento AFS", "ecogreencosmetico@gmail.com"));
            email.To.Add(new MailboxAddress(user.UserName, user.Email));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("ecogreencosmetico@gmail.com", "jnla qeey bbyg hhgr");
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }


    }
}
