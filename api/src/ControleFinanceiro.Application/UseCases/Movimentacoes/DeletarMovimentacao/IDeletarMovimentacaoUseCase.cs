namespace ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;

public interface IDeletarMovimentacaoUseCase
{
    Task<bool> ExecuteAsync(int id);
}