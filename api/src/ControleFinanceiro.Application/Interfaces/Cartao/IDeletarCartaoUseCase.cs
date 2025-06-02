namespace ControleFinanceiro.Application.Interfaces.Cartao;

public interface IDeletarCartaoUseCase
{
    Task<bool> ExecuteAsync(int id);
}