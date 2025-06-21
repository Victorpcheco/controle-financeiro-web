using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto
{
    public interface IListarMovimentacoesEmAbertoUseCase
    {
        Task<List<Movimentacoes>> ExecuteAsync();
    }
}
