using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentosAPI.Shared.Models.Modelos
{
    public class NivelAcesso
    {
        public string? Id { get;private set; }
        public string? TipoAcesso { get;private set; }
        public ICollection<User>? Users { get; set; }
        public NivelAcesso(){}
        public NivelAcesso(string id, string tipoAcesso)
        {
            Id = id ?? Guid.NewGuid().ToString();
            TipoAcesso = tipoAcesso;
        }
    }
}
