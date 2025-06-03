using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;

public class DeletarDespesaUseCase(IDespesaRepository repository) : IDeletarDespesaUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.");
        
        var despesa = await repository.BuscarDespesaPorIdAsync(id);
        
        if (despesa == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        await repository.RemoverDespesaAsync(despesa);
        return true;
    }
}