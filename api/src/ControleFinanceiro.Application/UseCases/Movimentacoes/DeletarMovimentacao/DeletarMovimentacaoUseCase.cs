using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;

public class DeletarMovimentacaoUseCase(IMovimentacoesRepository repository) : IDeletarMovimentacaoUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.");
        
        var movimentacoes = await repository.BuscarMovimentacoesPorIdAsync(id);
        
        if (movimentacoes == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        await repository.RemoverMovimentacoesAsync(movimentacoes);
        return true;
    }
}