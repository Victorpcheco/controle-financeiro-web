
using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IReadOnlyList<Categoria>> ListarCategoriaPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
        Task<int> ContarTotalAsync();
        Task<Categoria?> ObterCategoriaPorIdAsync(int id);
        Task<Categoria?> ObterCategoriaPorNomeAsync(string nome);
        Task CriarCategoriaAsync(Categoria categoria);
        Task AtualizarCategoriaAsync(Categoria categoria);
        Task DeletarCategoriaAsync(Categoria categoria);
    }
}
