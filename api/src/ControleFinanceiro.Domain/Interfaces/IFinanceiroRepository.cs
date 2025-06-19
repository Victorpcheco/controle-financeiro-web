using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces
{
    public interface IFinanceiroRepository
    {
        Task<decimal> ObterSaldoTotalAsync(int usuarioId);
        Task<List<SaldoContasPorUsuario>> ListarContasComSaldoTotalAsync(int usuarioId);
        Task<decimal> ObterValorEmAbertoDespesasAsync(int usuarioId);
        Task<decimal> ObterValorEmAbertoReceitasAsync(int usuarioId);
    }
}
