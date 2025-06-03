namespace ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;

public interface IDeletarDespesaUseCase
{
    Task<bool> ExecuteAsync(int id);
}