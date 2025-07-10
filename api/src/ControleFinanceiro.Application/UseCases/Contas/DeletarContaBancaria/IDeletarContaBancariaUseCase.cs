namespace ControleFinanceiro.Application.UseCases.Contas.DeletarContaBancaria;

public interface IDeletarContaBancariaUseCase
{
    Task<bool> ExecuteAsync(int id);
}