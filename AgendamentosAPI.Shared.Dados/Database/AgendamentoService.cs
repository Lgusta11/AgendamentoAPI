using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosAPI.Shared.Dados.Database
{
    public class AgendamentoService
    {
        private readonly AgendamentosContext _agendamentosContext;

        public AgendamentoService(AgendamentosContext agendamentosContext)
        {
            _agendamentosContext = agendamentosContext;
        }

        public IEnumerable<Agendamento> ListarAgendamentos()
        {
            try
            {
                var agendamentos = _agendamentosContext.Agendamentos
                .AsNoTracking()
                .Include(p => p.Professor)
                .Include(p => p.Equipamento)
                .Include(p => p.AgendamentoAulas)
                .ThenInclude(aa => aa.Aula!)
                .ToList();

                return agendamentos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
