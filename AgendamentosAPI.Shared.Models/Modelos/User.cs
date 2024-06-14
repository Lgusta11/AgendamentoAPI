using Agendamentos.Shared.Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentosAPI.Shared.Models.Modelos
{
    public class User
    {
        public string? Id { get; private set; }
        public string? UserName { get; private set; }
        public string? Email { get; private set; }
        public string? Senha { get; private set; }
        public string? AcessoId { get; private set; }
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
    }
}
