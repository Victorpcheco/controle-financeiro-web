using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/categoria")]
    public class CategoriaController(ICategoriaService service) : ControllerBase
    {

        [HttpGet("listarPaginado/{usuarioId:int}")]
        public async Task<IActionResult> ListarCategoriasPaginado(int usuarioId, [FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
        {
            var categorias = await service.ListarCategoriaPaginadoAsync(usuarioId,pagina, quantidade);

            return Ok(categorias);
        }

        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> ListarCategoriaPorId(int id)
        {
            var categoria = await service.BuscarCategoriaPorIdAsync(id);
            return Ok(categoria);
        }

        [HttpPost("criar/{usuarioId:int}")]
        public async Task<ActionResult> CriarCategoria(int usuarioId, [FromBody] CategoriaRequestDto dto)
        {
            await service.CriarCategoriaAsync(usuarioId, dto);
            return NoContent();
        }
        
        [HttpPut("atualizar/{id:int}")]
        public async Task<ActionResult> AtualizarCategoria(int id, [FromBody] CategoriaRequestDto dto)
        {
            await service.AtualizarCategoriaAsync(id, dto);
            return NoContent();
        }
        
        [HttpDelete("excluir/{id:int}")]
        public async Task<ActionResult> DeletarCategoria(int id)
        {
            await service.DeletarCategoriaAsync(id);
            return NoContent();
        }
    }
}
