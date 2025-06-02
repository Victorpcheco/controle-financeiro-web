using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
public class CategoriaController(IListarCategorias listar,
    IObterCategoria obter,
    ICriarCategoria criar,
    IAtualizarCategoria atualizar,
    IDeletarCategoria deletar) : ControllerBase
{

    [HttpGet("listarPaginado")]
    public async Task<IActionResult> ListarCategorias([FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
    {
        var categorias = await listar.ListarCategoriaPaginado(pagina, quantidade);
        return Ok(categorias);
    }

    [HttpGet("listar/{id:int}")]
    public async Task<IActionResult> ObterCategoria(int id)
    {
        var categoria = await obter.ObterCategoriaPorId(id);
        return Ok(categoria);
    }

    [HttpPost("criar")]
    public async Task<ActionResult> CriarCategoria([FromBody] CategoriaCriarRequest criarRequest)
    {
        await criar.CriarNovaCategoria(criarRequest);
        return NoContent();
    }
        
    [HttpPut("atualizar/{id:int}")]
    public async Task<ActionResult> AtualizarCategoria(int id, [FromBody] CategoriaCriarRequest criarRequest)
    {
        await atualizar.AtualizarCategoriaExistente(id, criarRequest);
        return NoContent();
    }
        
    [HttpDelete("deletar/{id:int}")]
    public async Task<ActionResult> DeletarCategoria(int id)
    {
        await deletar.DeletarCategoriaExistente(id);
        return NoContent();
    }
}