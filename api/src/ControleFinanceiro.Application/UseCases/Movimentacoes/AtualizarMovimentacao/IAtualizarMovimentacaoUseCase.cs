using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;

public interface IAtualizarMovimentacaoUseCase
{
    Task<bool> ExecuteAsync(int id, MovimentacaoCriarDto request);
}