﻿using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Todo.Shared.Models;
using Todo.Repository.Repositories.Contracts;
using Todo.Web.Commands;
using Todo.Web.Handlers;
using Todo.Repository.Repositories;
using Todo.Shared.Commands;
using Todo.Web.Handlers.Interfaces;
using Todo.Shared.ViewModel;

namespace Todo.Web.Controllers
{
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IHandler<CriarAtividadeCommand> _handlerCriarAtividade;
        private readonly IHandler<EditarAtividadeCommand> _handlerEditarAtividade;
        private readonly IHandler<ExcluirAtividadeCommand> _handlerExcluirAtividade;
        private readonly IHandler<ListarAtividadeCommand> _handlerListarAtividade;

        public TodoController(
                ITodoRepository todoRepository, 
                IHandler<CriarAtividadeCommand> handlerCriarAtividade,
                IHandler<EditarAtividadeCommand> handlerEditarAtividade,
                IHandler<ExcluirAtividadeCommand> handlerExcluirAtividade,
                IHandler<ListarAtividadeCommand> handlerListarAtividade
            )
        {
            _todoRepository = todoRepository;
            _handlerCriarAtividade = handlerCriarAtividade;
            _handlerEditarAtividade = handlerEditarAtividade;
            _handlerExcluirAtividade = handlerExcluirAtividade;
            _handlerListarAtividade = handlerListarAtividade;
        }

        [HttpGet("/")]
        public string ChamarApi()
        {
            Console.WriteLine("Chamou aqui");

            return "Está funcionando";
        }

        [HttpPost("v1/atividades/listar")]
        public async Task<ICommandResult> ListarAtividade(
                [FromBody] ListarAtividadeCommand atividade
            )
        {
            var acaoListarAtividade = await _handlerListarAtividade.Handle(atividade);

            return acaoListarAtividade;

        }

        [HttpGet("v1/atividades/listar/{id}")]
        public async Task<AtividadeViewModel?> ListarAtividadePorId(
                [FromRoute] int id
            )
        {
            var parametro = new
            {
                id
            };
            var retornoRepository = await _todoRepository.ListarAtividadePorIdAsync(parametro);

            //RespostaDados respostaDados = new RespostaDados();

            return retornoRepository;
            //return new CommandResult("Erro no envio dos dados", 401, command.Notifications);
        }

        [HttpPost("v1/atividades/criar")]
        public async Task<ICommandResult> CriarAtividade(
                [FromBody] CriarAtividadeCommand atividade
            )
        {
            var acaoCriarAtividade = await _handlerCriarAtividade.Handle(atividade);

            return acaoCriarAtividade;

        }

        [HttpPut("v1/atividades/editar")]
        public async Task<ICommandResult> EditarAtividade(
                [FromBody] EditarAtividadeCommand atividade
            )
        {
            var acaoEditarAtividade = await _handlerEditarAtividade.Handle(atividade);

            return acaoEditarAtividade;
        }

        [HttpDelete("v1/atividades/excluir")]
        public async Task<ICommandResult> ExcluirAtividade(
                [FromBody] ExcluirAtividadeCommand atividade
            )
        {
            var acaoExcluirAtividade = await _handlerExcluirAtividade.Handle(atividade);

            return acaoExcluirAtividade;
        }
    }
}
