using Agendamentos.Shared.Dados.Database;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentosAPI.Shared.Dados
{
    public class ProfessorService
    {
        private readonly AgendamentosContext _agendamentoContext;
        private readonly DbSet<User> _professores;

        public ProfessorService(AgendamentosContext agendamentoContext)
        {
            _agendamentoContext = agendamentoContext;
            _professores = _agendamentoContext.Users;
        }

        public async Task<IEnumerable<User>> ListarProfessores()
        {
            var professores = await _professores
                .AsNoTracking()
                .Include(p => p.NivelAcesso)
                .Where(p => p.NivelAcesso!.TipoAcesso == "Professor")
                .ToListAsync();

            return professores;
        }
        public async Task<User> BuscarProfessorPorId(Expression<Func<User, bool>> expression)
        {
            var professor = await _professores
                .AsNoTracking()
                .Include(p => p.NivelAcesso)
                .Where(expression)
                .FirstAsync();

            return professor;
        }
    }
}
