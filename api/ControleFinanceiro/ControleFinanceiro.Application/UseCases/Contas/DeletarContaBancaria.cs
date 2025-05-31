using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Contas;

public class DeletarContaBancaria(IContaBancariaRepository repository) : IDeletarContaBancaria
{
    public async Task<bool> DeletarConta(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await repository.BuscarContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        await repository.DeletarContaAsync(conta);
        return true;
    }
}