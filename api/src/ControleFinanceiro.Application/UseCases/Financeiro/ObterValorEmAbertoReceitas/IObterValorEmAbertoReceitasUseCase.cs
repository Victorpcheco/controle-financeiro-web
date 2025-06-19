namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterReceitasEmAberto
{
    public interface IObterValorEmAbertoReceitasUseCase
    {
        Task<decimal> ExecuteAsync();
    }
}
