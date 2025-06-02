namespace ControleFinanceiro.Application.Interfaces.Contas;

public interface IDeletarContaBancaria
{
    Task<bool> DeletarConta(int id);
}