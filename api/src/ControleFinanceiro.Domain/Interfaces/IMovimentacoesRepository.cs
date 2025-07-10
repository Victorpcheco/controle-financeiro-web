using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces
{
    public interface IMovimentacoesRepository
    {
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoAsync(int usuarioId, int pagina, int quantidade);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesReceitasPaginadoAsync(int usuarioId, int pagina, int quantidade);
        Task<int> ContarMovimentacoesDespesasAsync(int usuarioId);
        Task<int> ContarMovimentacoesReceitasAsync(int usuarioId);
        Task<Movimentacoes?> BuscarMovimentacoesPorIdAsync(int id);
        Task AdicionarMovimentacoesAsync(Movimentacoes Movimentacoes);
        Task AtualizarMovimentacoesAsync(Movimentacoes Movimentacoes);
        Task RemoverMovimentacoesAsync(Movimentacoes Movimentacoes);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade);
        Task<int> ContarMovimentacoesPorDataAsync(int usuarioId, DateOnly data);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade);
        Task<int> ContarMovimentacoesPorContaAsync(int usuarioId, int contaId);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade);
        Task<int> ContarMovimentacoesPorCategoriaAsync(int usuarioId, int categoriaId);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade);
        Task<int> ContarMovimentacoesPorTituloAsync(int usuarioId, string titulo);
        Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorCartaoAsync(int usuarioId, int cartaoId, int pagina, int quantidade);
        Task<int> ContarMovimentacoesPorCartaoAsync(int usuarioId, int cartaoId);
    }
}
