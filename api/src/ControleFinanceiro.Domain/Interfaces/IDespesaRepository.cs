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
}