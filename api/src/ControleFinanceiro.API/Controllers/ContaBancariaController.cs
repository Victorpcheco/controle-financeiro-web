using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Contas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/contas-bancarias")]
[Authorize]
public class ContaBancariaController(IListarContasBancarias listar,
    IObterContaBancaria obter,
    ICriarContaBancaria criar,
    IAtualizarContaBancaria atualizar,
    IDeletarContaBancaria deletar) : ControllerBase
{
    
    [HttpGet("listarPaginado")]
    public async Task<IActionResult> ListarContas([FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 15)
    {
        var contas = await listar.ListarContasPaginado(pagina, quantidadePorPagina);
        return Ok(contas);
    }
    
    [HttpGet("listar/{id:int}")]
    public async Task<IActionResult> ObterConta(int id)
    {
        var conta = await obter.ObterContaPorId(id);
        return Ok(conta);
    }
    
    [HttpPost("criar")]
    public async Task<IActionResult> CriarConta([FromBody] ContaRequest request)
    {
        await criar.CriarConta(request);
        return NoContent();
    }
    
    [HttpPut("atualizar/{id:int}")]
    public async Task<IActionResult> AtualizarConta(int id, [FromBody] ContaAtualizarRequest request)
    {
        await atualizar.AtualizarConta(id, request);
        return NoContent();
    }
    
    [HttpDelete("deletar/{id:int}")]
    public async Task<IActionResult> DeletarConta(int id)
    {
        await deletar.DeletarConta(id);
        return NoContent();
    }
}