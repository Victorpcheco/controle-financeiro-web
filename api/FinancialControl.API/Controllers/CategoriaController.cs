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
        public async Task<IActionResult> GetCategorias([FromQuery] int pagina = 1, [FromQuery] int quantidade = 15)
        {
            var (categorias, total) = await _categoriaService.ListarPaginadoAsync(pagina, quantidade);

            return Ok(new
            {
                Total = total,
                Pagina = pagina,
                Quantidade = quantidade,
                Categorias = categorias
            });
        }



    }
}
