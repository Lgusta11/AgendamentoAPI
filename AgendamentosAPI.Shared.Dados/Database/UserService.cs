using Agendamentos.Shared.Dados.Database;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentosAPI.Shared.Dados.Database
{
    public class UserService
    {
        private readonly AgendamentosContext _agendamentoContext;
        private readonly DbSet<User> _users;

        public UserService(AgendamentosContext agendamentoContext)
        {
            _agendamentoContext = agendamentoContext;
            _users = _agendamentoContext.Users;
        }

        public async Task<IEnumerable<User>> ListarUsers()
        {
            var professores = await _users
                .Include(p => p.NivelAcesso)
                .AsNoTracking()
                .ToListAsync();

            return professores;
        }
        public async Task<User> BuscarUserPorId(Expression<Func<User, bool>> expression)
        {
            var user = await _users
                .Include(p => p.NivelAcesso)
                .AsNoTracking()
                .FirstAsync(expression);

            return user;
        }

        public async Task AlterarUsuario(User user)
        {
            _agendamentoContext.Entry(user).State = EntityState.Modified;
            await _agendamentoContext.SaveChangesAsync();
        }
    }
}
