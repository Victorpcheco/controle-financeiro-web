using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.DTOs
{
    public class CategoriaPaginadoResponse
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public IReadOnlyList<CategoriaResponseDto?> Categorias { get; set; } 
    }
}
