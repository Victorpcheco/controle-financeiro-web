using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Contas.ListarContaBancaria;
using ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterDespesasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterReceitasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal;
using ControleFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class FinanceiroController(IObterSaldoTotalUseCase obterSaldoTotal,
        IListarContasComSaldoTotalUseCase obterSaldoConta,
        IObterValorEmAbertoDespesasUseCase obterDespesasEmAberto,
        IObterValorEmAbertoReceitasUseCase obterReceitasEmAberto,
        IListarMovimentacoesEmAbertoUseCase movimentacoesEmAberto) : ControllerBase
    {
        [HttpGet("saldo-total")]
        public async Task<ActionResult<decimal>> ObterSaldoTotal()
        {
            var saldo = await obterSaldoTotal.ExecuteAsync();
            return Ok(saldo);
        }

        [HttpGet("saldo-contas")]
        [ProducesResponseType(typeof(List<SaldoConta>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterSaldoPorConta()
        {
            var saldoConta = await obterSaldoConta.ExecuteAsync();
            return Ok(saldoConta);

        }

        [HttpGet("valor-em-aberto-despesas")]
        public async Task<ActionResult> ValorDespesasEmAberto()
        {
            var response = await obterDespesasEmAberto.ExecuteAsync();
            return Ok(response);
        }

        [HttpGet("valor-em-aberto-receitas")]
        public async Task<ActionResult> ValorReceitasEmAberto()
        {
            var response = await obterReceitasEmAberto.ExecuteAsync();
            return Ok(response);
        }

        [HttpGet("movimentacoes-dashboard")]
        public async Task<ActionResult> MovimentacoesEmAberto(
        [FromQuery] int usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
        {
            var request = new PaginadoRequestDto
            {
                Pagina = pagina,
                Quantidade = quantidade
            };

            var resultado = await movimentacoesEmAberto.ExecuteAsync(request);
            return Ok(resultado);
        }
    }
}
