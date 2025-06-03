using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;

public interface ICriarDespesaUseCase
{
    Task<bool> ExecuteAsync(DespesaCriarDto request);
}