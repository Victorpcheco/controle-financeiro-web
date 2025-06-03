using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Cartoes;
using ControleFinanceiro.Application.UseCases.Cartoes.AtualizarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.BuscarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.CriarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.DeletarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.ListarCartao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartaoController (
    IListarCartaoPaginadoUseCase listarCartaoPaginadoUseCase,
    IBuscarCartaoUseCase buscarCartaoUseCase,
    ICriarCartaoUseCase criarCartaoUseCase,
    IAtualizarCartaoUseCase atualizarCartaoUseCase,
    IDeletarCartaoUseCase deletarCartaoUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CartaoResponsePaginadoDto>> ListarCartaoAsync(
        [FromQuery] int usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };

        var resultado = await listarCartaoPaginadoUseCase.ExecuteAsync(request);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartaoResponseDto>> BuscarCartaoAsync(int id)
    {
        var resultado = await buscarCartaoUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CriarCartaoAsync([FromBody] CartaoCriarDto dto)
    {
        var resultado = await criarCartaoUseCase.ExecuteAsync(dto);
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> AtualizarCartaoAsync(int id, [FromBody] CartaoCriarDto dto)
    {
        var resultado = await atualizarCartaoUseCase.ExecuteAsync(id, dto);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeletarCartaoAsync(int id)
    {
        var resultado = await deletarCartaoUseCase.ExecuteAsync(id);
        return NoContent();
    }
}