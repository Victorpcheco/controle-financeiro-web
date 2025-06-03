using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.CriarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.DeletarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.AtualizarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.BuscarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.ListarMesReferencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MesReferenciaController(
    IListarMesReferenciaUseCase listarMesReferenciaPaginadoUseCase,
    IBuscarMesReferenciaUseCase buscarMesReferenciaUseCase,
    ICriarMesReferenciaUseCase criarMesReferenciaUseCase,
    IAtualizarMesReferenciaUseCase atualizarMesReferenciaUseCase,
    IDeletarMesReferenciaUseCase deletarMesReferenciaUseCase
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MesReferenciaPaginadoResponseDto>> ListarMesReferenciaAsync(
        [FromQuery] int usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        var resultado = await listarMesReferenciaPaginadoUseCase.ExecuteAsync(request);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MesReferenciaResponseDto>> BuscarMesReferenciaAsync(int id)
    {
        var resultado = await buscarMesReferenciaUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CriarMesReferenciaAsync([FromBody] MesReferenciaCriarDto dto)
    {
        var idCriado = await criarMesReferenciaUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(BuscarMesReferenciaAsync), new { id = idCriado }, idCriado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> AtualizarMesReferenciaAsync(int id, [FromBody] MesReferenciaCriarDto dto)
    {
        var resultado = await atualizarMesReferenciaUseCase.ExecuteAsync(id, dto);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeletarMesReferenciaAsync(int id)
    {
        var resultado = await deletarMesReferenciaUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }
}