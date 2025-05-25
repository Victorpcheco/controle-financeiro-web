using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers;
[ApiController]
[Route("api/contas")]
public class ContaBancariaController(IContaBancariaService service) : ControllerBase
{
    
    [HttpGet("listarPaginado/{usuarioId:int}")]
    public async Task<IActionResult> ListarContas(int usuarioId, [FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        var contas = await service.ListarContasPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        return Ok(contas);
    }
    
    [HttpGet("listar/{id:int}")]
    public async Task<IActionResult> ObterContaPorId(int id)
    {
        var conta = await service.ObterContaPorIdAsync(id);
        return Ok(conta);
    }
    
    [HttpPost("criar/{usuarioId:int}")]
    public async Task<IActionResult> CriarContaAsync(int usuarioId, [FromBody] ContaBancariaRequestDto dto)
    {
        await service.CriarContaAsync(usuarioId, dto);
        return NoContent();
    }
    
    [HttpPut("atualizar/{id:int}")]
    public async Task<IActionResult> AtualizarContaAsync(int id, [FromBody] ContaBancariaRequestDto dto)
    {
        await service.AtualizarContaAsync(id, dto);
        return NoContent();
    }
    
    [HttpDelete("deletar/{id:int}")]
    public async Task<IActionResult> DeletarContaAsync(int id)
    {
        await service.DeletarContaAsync(id);
        return NoContent();
    }
}