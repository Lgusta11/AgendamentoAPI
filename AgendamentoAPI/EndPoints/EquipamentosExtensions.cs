using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.EndPoints
{
    public static class EquipamentosExtensions
    {
    
            public static void AddEndPointsEquipamentos(this WebApplication app)
            {
                var groupBuilder = app.MapGroup("equipamentos")
                .WithTags("Equipamentos");


            groupBuilder.MapGet("",[Authorize] ([FromServices] DAL<Equipamentos> dal) =>
            {
                var listaDeEquipamentos = dal.Listar();
                if (listaDeEquipamentos is null)
                {
                    return Results.Ok(new { Info = "Nenhum equipamento encontrado!" });
                }
                var listaDeEquipamentosResponse = listaDeEquipamentos.Select(e => new EquipamentoResponse(e.Id, e.Nome, e.Quantidade)).ToList();
                return Results.Ok(listaDeEquipamentosResponse);
            });

                groupBuilder.MapGet("{id}",[Authorize] ([FromServices] DAL<Equipamentos> dal, int id) =>
                {
                    var equipamento = dal.RecuperarPor(e => e.Id == id);
                    if (equipamento is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(new EquipamentoResponse(equipamento.Id, equipamento.Nome, equipamento.Quantidade));
                });

            groupBuilder.MapPost("", [Authorize(Roles = "Gestor")] ([FromServices] DAL<Equipamentos> dal, [FromBody] EquipamentoRequest equipamentoRequest) =>
            {
                var equipamento = new Equipamentos(equipamentoRequest.Nome) { Quantidade = equipamentoRequest.Quantidade };
                dal.Adicionar(equipamento);
                return Results.Ok();
            });

            groupBuilder.MapDelete("{id}",[Authorize(Roles = "Gestor")] ([FromServices] DAL<Equipamentos> dal, int id) =>
            {
                var equipamento = dal.RecuperarPor(e => e.Id == id);
                if (equipamento is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(equipamento);
                return Results.NoContent();
            });

            groupBuilder.MapPut("{id}",[Authorize(Roles = "Gestor")] ([FromServices] DAL<Equipamentos> dal, [FromBody] EquipamentosRequestEdit equipamentoRequest) =>
            {
                var equipamentoAAtualizar = dal.RecuperarPor(e => e.Id == equipamentoRequest.Id);
                if (equipamentoAAtualizar is null)
                {
                    return Results.NotFound();
                }
                equipamentoAAtualizar.Nome = equipamentoRequest.Nome;
                equipamentoAAtualizar.Quantidade = equipamentoRequest.Quantidade;
                dal.Atualizar(equipamentoAAtualizar);
                return Results.Ok();
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

        }
    }
    }


