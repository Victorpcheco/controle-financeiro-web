using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<(IReadOnlyList<Categoria> Categorias, int Total)> ListarPaginadoAsync(int pagina, int quantidadePorPagina);



    }
}
