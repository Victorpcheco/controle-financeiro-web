using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IReadOnlyList<Categoria>> ListarCategoriaPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
    Task<int> ContarTotalAsync(int usuarioId);
    Task<Categoria?> BuscarCategoriaPorIdAsync(int id);
    Task<Categoria?> BuscarCategoriaPorNomeAsync(string nome);
    Task CriarCategoriaAsync(Categoria categoria);
    Task AtualizarCategoriaAsync(Categoria categoria);
    Task DeletarCategoriaAsync(Categoria categoria);
}