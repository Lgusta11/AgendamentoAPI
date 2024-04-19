using Microsoft.AspNetCore.Identity;

namespace Agendamentos.Shared.Dados.Modelos
{
    public class Admin : IdentityRole<int>
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }
}
