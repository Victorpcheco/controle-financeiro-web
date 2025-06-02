using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Cartao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartaoController : ControllerBase
{
    private readonly IListarCartaoPaginadoUseCase _listarCartaoPaginadoUseCase;
    private readonly IBuscarCartaoPorIdUseCase _buscarCartaoPorIdUseCase;
    private readonly ICriarCartaoUseCase _criarCartaoUseCase;
    private readonly IAtualizarCartaoUseCase _atualizarCartaoUseCase;
    private readonly IDeletarCartaoUseCase _deletarCartaoUseCase;

    public CartaoController(
        IListarCartaoPaginadoUseCase listarCartaoPaginadoUseCase,
        IBuscarCartaoPorIdUseCase buscarCartaoPorIdUseCase,
        ICriarCartaoUseCase criarCartaoUseCase,
        IAtualizarCartaoUseCase atualizarCartaoUseCase,
        IDeletarCartaoUseCase deletarCartaoUseCase)
    {
        _listarCartaoPaginadoUseCase = listarCartaoPaginadoUseCase;
        _buscarCartaoPorIdUseCase = buscarCartaoPorIdUseCase;
        _criarCartaoUseCase = criarCartaoUseCase;
        _atualizarCartaoUseCase = atualizarCartaoUseCase;
        _deletarCartaoUseCase = deletarCartaoUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<CartaoResponsePaginadoDto>> ListarCartaoPaginado(
        [FromQuery] int usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new ListarCartaoPaginadoRequest
        {
            Pagina = pagina,
            Quantidade = quantidade
        };

        var resultado = await _listarCartaoPaginadoUseCase.ExecuteAsync(request);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartaoResponseDto>> BuscarCartaoPorId(int id)
    {
        var resultado = await _buscarCartaoPorIdUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CriarCartao([FromBody] CartaoCriarDto dto)
    {
        var resultado = await _criarCartaoUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(BuscarCartaoPorId), new { id = 0 }, resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> AtualizarCartao(int id, [FromBody] CartaoCriarDto dto)
    {
        var resultado = await _atualizarCartaoUseCase.ExecuteAsync(id, dto);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeletarCartao(int id)
    {
        var resultado = await _deletarCartaoUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }
}