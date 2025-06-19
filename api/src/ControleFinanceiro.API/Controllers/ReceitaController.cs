using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Receitas.AtualizarReceita;
using ControleFinanceiro.Application.UseCases.Receitas.BuscarReceita;
using ControleFinanceiro.Application.UseCases.Receitas.CriarReceita;
using ControleFinanceiro.Application.UseCases.Receitas.DeletarReceita;
using ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorCategoria;
using ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorContaBancaria;
using ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorData;
using ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorTitulo;
using ControleFinanceiro.Application.UseCases.Receitas.ListarReceitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReceitaController(
    IListarReceitasUseCase listarReceitasUseCase,
    ICriarReceitaUseCase criarReceitaUseCase,
    IBuscarReceitaUseCase buscarReceitaUseCase,
IAtualizarReceitaUseCase atualizarReceitaUseCase,
    IDeletarReceitaUseCase deletarReceitaUseCase,
    IListarReceitaPorDataUseCase listarReceitaPorDataUseCase,
    IListarReceitaPorContaBancariaUseCase listarReceitaPorContaBancariaUseCase,
    IListarReceitaPorCategoriaUseCase listarReceitaPorCategoriaUseCase,
    IListarReceitaPorTituloUseCase listarReceitaPorTituloUseCase
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListarReceitas(
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        var response = await listarReceitasUseCase.ExecuteAsync(request);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> BuscarReceita(int id)
    {
        var response = await buscarReceitaUseCase.ExecuteAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CriarReceita([FromBody] ReceitaCriarDto request)
    {
        var response = await criarReceitaUseCase.ExecuteAsync(request);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> AtualizarReceita(int id, [FromBody] ReceitaCriarDto request)
    {
        var response = await atualizarReceitaUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletarReceita(int id)
    {
        await deletarReceitaUseCase.ExecuteAsync(id);
        return NoContent();
    }

    [HttpGet("por-data")]
    public async Task<ActionResult> ListarPorData(
        [FromQuery] DateOnly data,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto { Pagina = pagina, Quantidade = quantidade };
        var response = await listarReceitaPorDataUseCase.ExecuteAsync(data, request);
        return Ok(response);
    }

    [HttpGet("por-conta")]
    public async Task<ActionResult> ListarPorConta(
        [FromQuery] int contaId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto { Pagina = pagina, Quantidade = quantidade };
        var response = await listarReceitaPorContaBancariaUseCase.ExecuteAsync(contaId, request);
        return Ok(response);
    }

    [HttpGet("por-categoria")]
    public async Task<ActionResult> ListarPorCategoria(
        [FromQuery] int categoriaId,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto { Pagina = pagina, Quantidade = quantidade };
        var response = await listarReceitaPorCategoriaUseCase.ExecuteAsync(categoriaId, request);
        return Ok(response);
    }

    [HttpGet("por-titulo")]
    public async Task<ActionResult> ListarPorTitulo(
        [FromQuery] string titulo,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto { Pagina = pagina, Quantidade = quantidade };
        var response = await listarReceitaPorTituloUseCase.ExecuteAsync(titulo, request);
        return Ok(response);
    }
}