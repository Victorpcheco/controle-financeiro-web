using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Categorias.AtualizarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.BuscarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.CriarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.DeletarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.ListarCategorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController(
    IListarCategoriasUseCase listarCategoriasUseCase,
    IBuscarCategoriaUseCase buscarCategoriaUseCase,
    ICriarCategoriaUseCase criarCategoriaUseCase,
    IAtualizarCategoriaUseCase atualizarCategoriaUseCase,
    IDeletarCategoriaUseCase deletarCategoriaUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CategoriaPaginadoResponseDto>> ListarCategoriasAsync(
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };

        var resultado = await listarCategoriasUseCase.ExecuteAsync(request);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaResponseDto>> BuscarCategoriaAsync(int id)
    {
        var resultado = await buscarCategoriaUseCase.ExecuteAsync(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CriarCategoriaAsync([FromBody] CategoriaCriarDto dto)
    {
        var resultado = await criarCategoriaUseCase.ExecuteAsync(dto);
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> AtualizarCategoriaAsync(int id, [FromBody] CategoriaCriarDto dto)
    {
        var resultado = await atualizarCategoriaUseCase.ExecuteAsync(id, dto);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeletarCategoriaAsync(int id)
    {
        var resultado = await deletarCategoriaUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
