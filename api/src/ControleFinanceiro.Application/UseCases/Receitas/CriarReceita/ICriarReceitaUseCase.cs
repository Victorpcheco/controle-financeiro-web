using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.CriarReceita;

public interface ICriarReceitaUseCase
{
    Task<bool> ExecuteAsync(ReceitaCriarDto request);
}