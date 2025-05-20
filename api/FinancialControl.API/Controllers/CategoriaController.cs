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

        [HttpGet("listarPaginado/{usuarioId:int}")]
        public async Task<IActionResult> ListarCategoriasPaginado(int usuarioId, [FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
        {
            var categorias = await _categoriaService.ListarPaginadoAsync(usuarioId,pagina, quantidade);

            return Ok(categorias);
        }

        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> ListarCategoriaPorId(int id)
        {
            var categoria = await _categoriaService.BuscarPorIdAsync(id);
            return Ok(categoria);
        }

        [HttpPost("criar/{usuarioId:int}")]
        public async Task<ActionResult> CriarCategoria(int usuarioId, [FromBody] CategoriaRequestDto dto)
        {
            await _categoriaService.CriarCategoriaAsync(usuarioId, dto);
            return NoContent();
        }
        [HttpPut("atualizar/{id:int}")]
        public async Task<ActionResult> AtualizarCategoria(int id, [FromBody] CategoriaRequestDto dto)
        {
            await _categoriaService.AtualizarCategoriaAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("deletar/{id:int}")]
        public async Task<ActionResult> DeletarCategoria(int id)
        {
            await _categoriaService.DeletarCategoriaAsync(id);
            return NoContent();
        }
    }
}
