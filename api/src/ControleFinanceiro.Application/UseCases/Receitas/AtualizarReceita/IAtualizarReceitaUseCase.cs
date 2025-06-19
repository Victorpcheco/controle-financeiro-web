using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.AtualizarReceita;

public interface IAtualizarReceitaUseCase
{
    Task<bool> ExecuteAsync(int id, ReceitaCriarDto request);
}