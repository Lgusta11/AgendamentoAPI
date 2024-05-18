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





                groupBuilder.MapGet("", async ([FromServices] DAL<Agendamento> dal, [FromServices] DAL<Professores> professorDal, DAL<Aulas> aulasDal, DAL<Equipamentos> equipamentosDal) =>
                {
                    var listaDeAgendamentos = dal.Listar();
                    if (listaDeAgendamentos is null)
                    {
                        return Results.NotFound();
                    }
                    var listaDeAgendamentosResponse = new List<AgendamentoResponse>();
                    foreach (var agendamento in listaDeAgendamentos)
                    {
                        var professor = professorDal.RecuperarPor(p => p.Id == agendamento.ProfessorId);
                        var aula = aulasDal.RecuperarPor(a => a.Id == agendamento.AulaId);
                        var equipamento = equipamentosDal.RecuperarPor(e => e.Id == agendamento.EquipamentoId);
                        if (professor != null && aula != null && equipamento != null)
                        {
                            var agendamentoResponse = new AgendamentoResponse(agendamento.Id, agendamento.Data, aula.Aula, equipamento.Nome, professor.Nome);
                            listaDeAgendamentosResponse.Add(agendamentoResponse);
                        }
                    }
                    return Results.Ok(listaDeAgendamentosResponse);
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });



                groupBuilder.MapGet("{id}", async ([FromServices] DAL<Agendamento> dal, [FromServices] DAL<Professores> professorDal, DAL<Aulas> aulasDal, DAL<Equipamentos> equipamentosDal, int professorId) =>
                {
                    var agendamentosDoProfessor = dal.Listar(a => a.ProfessorId == professorId);
                    if (!agendamentosDoProfessor.Any())
                    {
                        return Results.NotFound();
                    }
                    var listaDeAgendamentosResponse = new List<AgendamentoResponse>();
                    foreach (var agendamento in agendamentosDoProfessor)
                    {
                        var professor = professorDal.RecuperarPor(p => p.Id == agendamento.ProfessorId);
                        var aula = aulasDal.RecuperarPor(a => a.Id == agendamento.AulaId);
                        var equipamento = equipamentosDal.RecuperarPor(e => e.Id == agendamento.EquipamentoId);
                        if (professor != null && aula != null && equipamento != null)
                        {
                            var agendamentoResponse = new AgendamentoResponse(agendamento.Id, agendamento.Data, aula.Aula, equipamento.Nome, professor.Nome);
                            listaDeAgendamentosResponse.Add(agendamentoResponse);
                        }
                    }
                    return Results.Ok(listaDeAgendamentosResponse);
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


                    return Results.Created();
                });



                groupBuilder.MapDelete("{id}", ([FromServices] DAL<Agendamento> dal, int id, [FromServices] PessoaComAcesso user) =>
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