using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;

public interface ICriarMovimentacaoUseCase
{
    Task<bool> ExecuteAsync(MovimentacaoCriarDto request);
}