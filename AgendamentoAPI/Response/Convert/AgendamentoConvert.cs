using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agendamentos.Response;
using Agendamentos.Shared.Modelos.Modelos;

namespace AgendamentoAPI.Response.Convert
{
    public static class AgendamentoConvert
    {
        public static AgendamentoResponse ToResponse(this Agendamento agendamento)
        {
            return new AgendamentoResponse
            {
                Id = agendamento.Id,
                Data = agendamento.Data,
                Equipamento = new EquipamentoResponse(
                   agendamento.Equipamento.Id,
                   agendamento.Equipamento.Nome,
                   agendamento.Equipamento.Quantidade
                ),
                Professor = new ProfessoresResponse(
                    agendamento.Professor.Id,
                    agendamento.Professor.Nome,
                    agendamento.Professor.Email
                ),
                Aulas = agendamento.AgendamentoAulas.Select(agenda => new AgendamentoAulaResponse
                {
                    Id = agenda.AulaId,
                    Aula = "Aula teste",
                    Data = DateTime.Now
                }).ToList()
            };
        }
    }
}