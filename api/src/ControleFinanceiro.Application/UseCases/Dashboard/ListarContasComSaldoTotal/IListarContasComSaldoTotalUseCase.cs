using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas
{
    public interface IListarContasComSaldoTotalUseCase
    {
        Task<List<SaldoConta>> ExecuteAsync();
    }
}
