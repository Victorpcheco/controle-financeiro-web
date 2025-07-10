namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal
{
    public interface IObterSaldoTotalUseCase
    {
        Task<decimal> ExecuteAsync();
    }
}
