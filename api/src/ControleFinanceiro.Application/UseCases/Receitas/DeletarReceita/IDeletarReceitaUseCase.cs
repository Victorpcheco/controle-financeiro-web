namespace ControleFinanceiro.Application.UseCases.Receitas.DeletarReceita;

public interface IDeletarReceitaUseCase
{
    Task<bool> ExecuteAsync(int id);
}