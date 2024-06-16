using Agendamentos.Shared.Modelos.Modelos;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendamentosAPI.Shared.Models.Modelos
{
    public class User
    {
        public string? Id { get;  set; }
        public string? UserName { get;  set; }
        public string? Email { get;  set; }
        public string? Senha { get;  set; }
        public string? AcessoId { get;  set; }
        public string? Token { get; set; }
        public NivelAcesso? NivelAcesso { get; set; }
        public ICollection<Agendamento>? Agendamentos { get; set; }

        public User()
        {

        }
        public User(string id, string userName, string email, string senha, string acessoId)
        {
            Id = id ?? Guid.NewGuid().ToString();
            UserName = userName;
            Email = email;
            Senha = senha;
            AcessoId = acessoId;
        }

        public void AdicionarToken(string token)
        {
            Token = token;
        }
        public void AlterarToken(string token)
        {
            Token = token;
        }
    }
}