using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.DeletarReceita;

public class DeletarReceitaUseCase(IReceitaRepository repository) : IDeletarReceitaUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.");

        var receita = await repository.BuscarReceitaPorIdAsync(id);

        if (receita == null)
            throw new KeyNotFoundException("Receita não encontrada.");

        await repository.RemoverReceitaAsync(receita);
        return true;
    }
}