using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;

public interface IAtualizarDespesaUseCase
{
    Task<bool> ExecuteAsync(int id, DespesaCriarDto request);
}