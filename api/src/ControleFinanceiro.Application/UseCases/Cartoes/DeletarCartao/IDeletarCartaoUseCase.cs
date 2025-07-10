namespace ControleFinanceiro.Application.UseCases.Cartoes.DeletarCartao;

public interface IDeletarCartaoUseCase
{
    Task<bool> ExecuteAsync(int id);
}