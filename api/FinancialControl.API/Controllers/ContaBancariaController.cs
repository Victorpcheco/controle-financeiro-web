using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers;
[ApiController]
[Route("api/contas-bancarias")]
public class ContaBancariaController : ControllerBase
{
    private readonly IContaBancariaService _service;

    public ContaBancariaController(IContaBancariaService service)
    {
        _service = service;
    }
    
    [HttpGet("listarPaginado/{usuarioId:int}")]
    public async Task<IActionResult> ListarContasBancarias(int usuarioId, [FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        var contas = await _service.ListarContasPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        return Ok(contas);
    }
    
    [HttpGet("listar/{id:int}")]
    public async Task<IActionResult> BuscarContaPorIdAsync(int id)
    {
        var conta = await _service.BuscarContaPorIdAsync(id);
        return Ok(conta);
    }
    
    [HttpPost("criar/{usuarioId:int}")]
    public async Task<IActionResult> CriarContaAsync(int usuarioId, [FromBody] ContaBancariaRequestDto dto)
    {
        await _service.CriarContaAsync(usuarioId, dto);
        return NoContent();
    }
    
    [HttpPut("atualizar/{id:int}")]
    public async Task<IActionResult> AtualizarContaAsync(int id, [FromBody] ContaBancariaRequestDto dto)
    {
        await _service.AtualizarContaAsync(id, dto);
        return NoContent();
    }
    
    [HttpDelete("deletar/{id:int}")]
    public async Task<IActionResult> DeletarContaAsync(int id)
    {
        var resultado = await _service.DeletarContaAsync(id);
        return NoContent();
    }
}