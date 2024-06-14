using Agendamentos.Shared.Dados.Database;
using Microsoft.EntityFrameworkCore;


namespace AgendamentosAPI.Shared.Dados.Database.Auth
{
    public class AuthService
    {
        private readonly AgendamentosContext _dbContext;
        public AuthService(AgendamentosContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ValidarLogin(string email, string senha)
        {
            var usuarioExists = await _dbContext.Users.AnyAsync(u => u.Email == email && u.Senha == senha);

            return usuarioExists;
        }

        public async Task<string> BuscarAcesso(string email, string senha)
        {
            var usuarioAcesso = await _dbContext.Users
                .Include(p => p.NivelAcesso)
                .Where(p => p.Email == email &&  p.Senha == senha)
                .Select(p => p.NivelAcesso!.TipoAcesso)
                .FirstAsync();

            return usuarioAcesso!;
        }
    }
}
