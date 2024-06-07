using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;
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
                    if (listaDeAgendamentos is null || !listaDeAgendamentos.Any())
                    {
                        return Results.NotFound("Nenhum agendamento encontrado.");
                    }

                    var listaDeAgendamentosResponse = new List<AgendamentoResponse>();
                    foreach (var agendamento in listaDeAgendamentos)
                    {
                        var professor = professorDal.RecuperarPor(p => p.Id == agendamento.ProfessorId);
                        var equipamento = equipamentosDal.RecuperarPor(e => e.Id == agendamento.EquipamentoId);

                        if (professor == null)
                        {
                            return Results.NotFound($"Professor com ID {agendamento.ProfessorId} não encontrado.");
                        }
                        if (equipamento == null)
                        {
                            return Results.NotFound($"Equipamento com ID {agendamento.EquipamentoId} não encontrado.");
                        }

                        foreach (var aulaAgendada in agendamento.AgendamentoAulas)
                        {
                            var aula = aulasDal.RecuperarPor(a => a.Id == aulaAgendada.AulaId);
                            if (aula == null)
                            {
                                return Results.NotFound($"Aula com ID {aulaAgendada.AulaId} não encontrada.");
                            }

                            var agendamentoResponse = new AgendamentoResponse(agendamento.Id, agendamento.Data, aula.Aula, equipamento.Nome, professor.Nome);
                            listaDeAgendamentosResponse.Add(agendamentoResponse);
                        }
                    }

                    return Results.Ok(listaDeAgendamentosResponse);
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

                groupBuilder.MapGet("{professorId}", async ([FromServices] DAL<Agendamento> dal, [FromServices] DAL<Professores> professorDal, DAL<Aulas> aulasDal, DAL<Equipamentos> equipamentosDal, int professorId) =>
                {
                    var agendamentosDoProfessor = dal.Listar(a => a.ProfessorId == professorId);
                    if (agendamentosDoProfessor == null || !agendamentosDoProfessor.Any())
                    {
                        return Results.NotFound($"Nenhum agendamento encontrado para o professor com ID {professorId}.");
                    }

                    var listaDeAgendamentosResponse = new List<AgendamentoResponse>();
                    foreach (var agendamento in agendamentosDoProfessor)
                    {
                        var professor = professorDal.RecuperarPor(p => p.Id == agendamento.ProfessorId);
                        var equipamento = equipamentosDal.RecuperarPor(e => e.Id == agendamento.EquipamentoId);

                        if (professor == null)
                        {
                            return Results.NotFound($"Professor com ID {agendamento.ProfessorId} não encontrado.");
                        }
                        if (equipamento == null)
                        {
                            return Results.NotFound($"Equipamento com ID {agendamento.EquipamentoId} não encontrado.");
                        }

                        foreach (var aulaAgendada in agendamento.AgendamentoAulas)
                        {
                            var aula = aulasDal.RecuperarPor(a => a.Id == aulaAgendada.AulaId);
                            if (aula == null)
                            {
                                return Results.NotFound($"Aula com ID {aulaAgendada.AulaId} não encontrada.");
                            }

                            var agendamentoResponse = new AgendamentoResponse(agendamento.Id, agendamento.Data, aula.Aula, equipamento.Nome, professor.Nome);
                            listaDeAgendamentosResponse.Add(agendamentoResponse);
                        }
                    }

                    return Results.Ok(listaDeAgendamentosResponse);
                });






                groupBuilder.MapPost("", async ([FromServices] DAL<Agendamento> dal, [FromServices] DAL<Equipamentos> EquipamentosDal, [FromBody] AgendamentoRequest agendamentoRequest, [FromServices] PessoaComAcesso user) =>
                {
                    if (agendamentoRequest == null)
                    {
                        return Results.BadRequest("O pedido de agendamento é nulo.");
                    }

                    if (agendamentoRequest.AulaIds == null || !agendamentoRequest.AulaIds.Any())
                    {
                        return Results.BadRequest("Nenhuma aula foi fornecida no pedido de agendamento.");
                    }

                    var equipamento = await EquipamentosDal.RecuperarPorAsync(e => e.Id == agendamentoRequest.EquipamentoId);

                    if (equipamento == null)
                    {
                        return Results.BadRequest("O equipamento não foi encontrado.");
                    }

                    List<string> errors = new List<string>();
                    bool agendamentoRealizado = false;

                    foreach (var aulaId in agendamentoRequest.AulaIds)
                    {
                        // Verificar se o professor já tem um agendamento para a mesma aula no mesmo dia com o mesmo equipamento
                        var agendamentoExistenteProfessor = dal.Listar(a => a.ProfessorId == agendamentoRequest.ProfessorId && a.Data == agendamentoRequest.Data && a.EquipamentoId == agendamentoRequest.EquipamentoId && a.AgendamentoAulas.Any(aa => aa.AulaId == aulaId));
                        if (agendamentoExistenteProfessor.Any())
                        {
                            errors.Add($"Não foi possível fazer o agendamento da aula {aulaId} neste equipamento e data.");
                            continue;
                        }

                        try
                        {
                            var agendamento = new Agendamento { Data = agendamentoRequest.Data, EquipamentoId = agendamentoRequest.EquipamentoId, ProfessorId = agendamentoRequest.ProfessorId };
                            agendamento.AgendamentoAulas.Add(new AgendamentoAula { AulaId = aulaId });
                            dal.Adicionar(agendamento);
                            agendamentoRealizado = true;
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Erro ao criar agendamento para a aula {aulaId}: {ex.Message}");
                        }
                    }

                    if (errors.Any() && agendamentoRealizado)
                    {
                        return Results.BadRequest("Foi possível fazer o agendamento de algumas aulas, mas ocorreram os seguintes erros: " + string.Join(", ", errors));
                    }
                    else if (errors.Any())
                    {
                        return Results.BadRequest("Não foi possível fazer o agendamento de nenhuma aula. Os seguintes erros ocorreram: " + string.Join(", ", errors));
                    }

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
                    agendamentoAAtualizar.EquipamentoId = agendamentoRequestEdit.EquipamentoId;
                    agendamentoAAtualizar.ProfessorId = agendamentoRequestEdit.ProfessorId;

                    // Atualizar cada agendamento com a nova lista de aulas
                    foreach (var aulaId in agendamentoRequestEdit.AulaIds)
                    {
                        agendamentoAAtualizar.AgendamentoAulas.Add(new AgendamentoAula { AulaId = aulaId });
                        dal.Atualizar(agendamentoAAtualizar);
                    }

                    return Results.Ok();
                });
            }
        }


    }
}
