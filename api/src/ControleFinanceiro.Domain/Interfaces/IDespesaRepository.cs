using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces;

public interface IDespesaRepository
{
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoAsync(int usuarioId, int pagina, int quantidade);
    Task<int> ContarDespesasAsync(int usuarioId);
    Task<Despesa?> BuscarDespesaPorIdAsync(int id);
    Task AdicionarDespesaAsync(Despesa despesa);
    Task AtualizarDespesaAsync(Despesa despesa);
    Task RemoverDespesaAsync(Despesa despesa);
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade);
    Task<int> ContarDespesasPorDataAsync(int usuarioId, DateOnly data);
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade);
    Task<int> ContarDespesasPorContaAsync(int usuarioId, int contaId);
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade);
    Task<int> ContarDespesasPorCategoriaAsync(int usuarioId, int categoriaId);
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade);
    Task<int> ContarDespesasPorTituloAsync(int usuarioId, string titulo);
    Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorCartaoAsync(int usuarioId, int cartaoId, int pagina, int quantidade);
    Task<int> ContarDespesasPorCartaoAsync(int usuarioId, int cartaoId);
}