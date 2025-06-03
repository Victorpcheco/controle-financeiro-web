using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Contas.AtualizarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.BuscarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.CriarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.DeletarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.ListarContaBancaria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContaBancariaController (
    IListarContasBancariasUseCase listarContasBancariasUseCase,
    IBuscarContaBancariaUseCase buscarContaBancaria,
    ICriarContaBancariaUseCase criarContaBancariaUseCase,
    IAtualizarContaBancariaUseCase atualizarContaBancariaUseCase,
    IDeletarContaBancariaUseCase deletarContaBancariaUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ContaBancariaPaginadoResponseDto>> ListarContasAsync(
        [FromQuery] int usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };

        var resultado = await listarContasBancariasUseCase.ExecuteAsync(request);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContaBancariaResponseDto>> BuscarContaBancariaAsync(int id)
    {
        var resultado = await buscarContaBancaria.ExecuteAsync(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CriarContaAsync([FromBody] ContaBancariaCriarDto dto)
    {
        var resultado = await criarContaBancariaUseCase.ExecuteAsync(dto);
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> AtualizarContaAsync(int id, [FromBody] ContaBancariaAtualizarDto dto)
    {
        var resultado = await atualizarContaBancariaUseCase.ExecuteAsync(id, dto);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeletarContaAsync(int id)
    {
        var resultado = await deletarContaBancariaUseCase.ExecuteAsync(id);
        return NoContent();
    }
}