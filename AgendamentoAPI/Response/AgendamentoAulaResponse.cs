using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendamentoAPI.Response
{
    public class AgendamentoAulaResponse
    {
        public int Id { get; set; }
        public string? Aula { get; set; }
        public DateTime Data { get; set; }
    }
}