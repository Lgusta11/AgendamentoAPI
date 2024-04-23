using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.EndPoints
{
    public static class AgendamentosExtensions
    {
        public static void AddEndPointsAgendamentos(this WebApplication app)
        {
            {
                var groupBuilder = app.MapGroup("agendamentos")
                .WithTags("Agendamentos");


               
                groupBuilder.MapGet("", ([FromServices] DAL<Agendamento> dal) =>
                {
                    var listaDeAgendamentos = dal.Listar();
                    if (listaDeAgendamentos is null)
                    {
                        return Results.NotFound();
                    }
                    var listaDeAgendamentosResponse = listaDeAgendamentos.Select(a => new AgendamentoResponse(a.Id, a.Data, a.AulaId, a.EquipamentoId, a.ProfessorId)).ToList();
                    return Results.Ok(listaDeAgendamentosResponse);
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

               groupBuilder.MapGet("{id}", ([FromServices] DAL<Agendamento> dal, int professorId) =>
{
               var agendamentosDoProfessor = dal.Listar(a => a.ProfessorId == professorId);
                if (!agendamentosDoProfessor.Any())
                {
                    return Results.NotFound();
                                                    }
                        return Results.Ok(agendamentosDoProfessor.Select(a => new AgendamentoResponse(a.Id, a.Data, a.AulaId, a.EquipamentoId, a.ProfessorId)));
                });


                groupBuilder.MapPost("", async ([FromServices] DAL<Agendamento> dal, [FromServices] DAL<Equipamentos> EquipamentosDal, [FromBody] AgendamentoRequest agendamentoRequest, [FromServices] PessoaComAcesso user) =>
                {
                    var agendamentosExistenteEquipamento = dal.Listar(a => a.EquipamentoId == agendamentoRequest.EquipamentoId && a.Data == agendamentoRequest.Data);
                    var equipamento = await EquipamentosDal.RecuperarPorAsync(e => e.Id == agendamentoRequest.EquipamentoId);

                    // Verificar se o professor já tem um agendamento para a mesma aula no mesmo dia
                    var agendamentoExistenteProfessor = dal.Listar(a => a.ProfessorId == agendamentoRequest.ProfessorId && a.Data == agendamentoRequest.Data && a.AulaId == agendamentoRequest.AulaId);
                    if (agendamentoExistenteProfessor.Any())
                    {
                        return Results.BadRequest("O professor já tem um agendamento para a mesma aula nesta data.");
                    }

                    if (agendamentosExistenteEquipamento.Count() >= equipamento?.Quantidade)
                    {
                        return Results.BadRequest("O equipamento já está totalmente agendado para esta data e horário.");
                    }

                    var agendamento = new Agendamento { Data = agendamentoRequest.Data, AulaId = agendamentoRequest.AulaId, EquipamentoId = agendamentoRequest.EquipamentoId, ProfessorId = agendamentoRequest.ProfessorId };
                    dal.Adicionar(agendamento);

                  
                    return Results.Ok();
                });



                groupBuilder.MapDelete("{id}",  ([FromServices] DAL<Agendamento> dal, int id, [FromServices] PessoaComAcesso user) =>
                {
                    var agendamento = dal.RecuperarPor(a => a.Id == id);
                    if (agendamento is null)
                    {
                        return Results.NotFound();
                    }
                    dal.Deletar(agendamento);

                  
                    return Results.NoContent();
                });




                groupBuilder.MapPut("", ([FromServices] DAL<Agendamento> dal, [FromBody] AgendamentosRequestEdit agendamentoRequestEdit) => {
                    var agendamentoAAtualizar = dal.RecuperarPor(a => a.Id == agendamentoRequestEdit.Id);
                    if (agendamentoAAtualizar is null)
                    {
                        return Results.NotFound();
                    }
                    agendamentoAAtualizar.Data = agendamentoRequestEdit.Data;
                    agendamentoAAtualizar.AulaId = agendamentoRequestEdit.AulaId;
                    agendamentoAAtualizar.EquipamentoId = agendamentoRequestEdit.EquipamentoId;
                    agendamentoAAtualizar.ProfessorId = agendamentoRequestEdit.ProfessorId;
                    dal.Atualizar(agendamentoAAtualizar);
                    return Results.Ok();
                });
            }
        }

       
    }
}
