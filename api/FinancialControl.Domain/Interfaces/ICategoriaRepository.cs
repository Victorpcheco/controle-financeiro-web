
using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IReadOnlyList<Categoria>> ListarPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
        Task<int> ContarTotalAsync();
        Task<Categoria> BuscarPorId(int id);
        Task<Categoria> BuscarPorNome(string nome);
        Task CriarCategoriaAsync(Categoria categoria);
        Task AtualizarCategoriaAsync(Categoria categoria);
        Task DeletarCategoriaAsync(Categoria categoria);
    }
}
