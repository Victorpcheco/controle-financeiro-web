using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/categoria")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarCategoriasPaginado([FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
        {
            var categorias = await _categoriaService.ListarPaginadoAsync(pagina, quantidade);

            return Ok(categorias);
        }

        [HttpGet("Listar/{id:int}")]
        public async Task<IActionResult> ListarCategoriaPorId(int id)
        {
            var categoria = await _categoriaService.BuscarPorIdAsync(id);
            return Ok(categoria);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult> CriarCategoria([FromBody] CategoriaRequestDto dto)
        {
            await _categoriaService.CriarCategoriaAsync(dto);
            return NoContent();
        }
        [HttpPut("Atualizar/{id:int}")]
        public async Task<ActionResult> AtualizarCategoria( [FromRoute] int id, [FromBody] CategoriaRequestDto dto)
        {
            await _categoriaService.AtualizarCategoriaAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("Deletar/{id:int}")]
        public async Task<ActionResult> DeletarCategoria(int id)
        {
            await _categoriaService.DeletarCategoriaAsync(id);
            return NoContent();
        }
    }
}
