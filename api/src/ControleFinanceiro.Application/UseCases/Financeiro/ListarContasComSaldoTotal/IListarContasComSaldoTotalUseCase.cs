using ControleFinanceiro.Application.DTOs;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas
{
    public interface IListarContasComSaldoTotalUseCase
    {
        Task<ListarSaldosContasResponse> ExecutarAsync();
    }
}
