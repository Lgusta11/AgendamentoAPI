using Agendamentos.Shared.Dados.Database;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosAPI.Shared.Dados.Database
{
    public class NiveisdeAcessoService
    {
        private readonly AgendamentosContext _agendamentosContext;

        public NiveisdeAcessoService(AgendamentosContext agendamentosContext)
        {
            _agendamentosContext = agendamentosContext;
        }

        public async Task<IEnumerable<NivelAcesso>> ListarNiveisDeAcesso()
        {
            var niveisAcesso = await _agendamentosContext.NivelAcessos.ToListAsync();

            return niveisAcesso;
        }
        public async Task<NivelAcesso> BuscarNivelDeAcesso(string id)
        {
            var nivelAcesso = await _agendamentosContext.NivelAcessos.FirstAsync(p => p.Id == id);

            return nivelAcesso;
        }
    }
}