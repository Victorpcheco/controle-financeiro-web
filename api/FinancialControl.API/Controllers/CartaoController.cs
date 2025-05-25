using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers;


[ApiController]
[Route("api/cartao")]
public class CartaoController(ICartaoService service) : ControllerBase
{
    [HttpGet("listarPaginado/{usuarioId:int}")]
    public async Task<IActionResult> ListarCartoesPaginado(int usuarioId, [FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
    {
        var cartoes = await service.ListarCartaoPaginadoAsync(usuarioId, pagina, quantidade);
        return Ok(cartoes);
    }   
    
    [HttpGet("listar/{id:int}")]
    public async Task<IActionResult> ListarCartaoPorId(int id)
    {
        var cartoes = await service.BuscarCartaoPorIdAsync(id);
        return Ok(cartoes);
    }
    
    [HttpPost("Criar{usuarioId:int}")]
    public async Task<IActionResult> CriarCartao(int usuarioId, [FromBody] CartaoRequestDto cartao)
    { 
        await service.CriarCartaoAsync(usuarioId, cartao);
        return NoContent();
    }
    
    [HttpPut("Atualizar/{id:int}")]
    public async Task<IActionResult> AtualizarCartao(int id, [FromBody] CartaoRequestDto cartao)
    {
        await service.AtualizarCartaoAsync(id, cartao);
        return NoContent();
    }
    
    [HttpDelete("Deletar/{id:int}")]
    public async Task<IActionResult> DeletarCartao(int id)
    {
        await service.DeletarCartaoAsync(id);
        return NoContent();
    }
}