using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterDespesasEmAberto
{
    public interface IObterValorEmAbertoDespesasUseCase
    {
        Task<decimal> ExecuteAsync();
    }
}
