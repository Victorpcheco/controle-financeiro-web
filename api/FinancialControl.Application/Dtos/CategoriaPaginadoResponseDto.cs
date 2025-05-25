
namespace FinancialControl.Application.DTOs
{
    public class CategoriaPaginadoResponseDto
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public IReadOnlyList<CategoriaResponseDto?> Categorias { get; set; } = null!;
    }
}
