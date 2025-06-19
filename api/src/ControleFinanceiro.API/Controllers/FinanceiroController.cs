using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterDespesasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterReceitasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers
{
    [ApiController]
    [Route("api/financeiro")]
    [Authorize]
    public class FinanceiroController(IObterSaldoTotalUseCase obterSaldoTotal,
        IListarContasComSaldoTotalUseCase obterSaldoConta,
        IObterValorEmAbertoDespesasUseCase obterDespesasEmAberto,
        IObterValorEmAbertoReceitasUseCase obterReceitasEmAberto) : ControllerBase
    {
        [HttpGet("saldo-total")]
        public async Task<ActionResult<decimal>> ObterSaldoTotal()
        {
            var saldo = await obterSaldoTotal.ExecuteAsync();
            return Ok(saldo);
        }

        [HttpGet("saldo-contas")]
        public async Task<ActionResult<SaldoContaDto>> ObterSaldoConta()
        {
            var saldoConta = await obterSaldoConta.ExecutarAsync();
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
    }
}
