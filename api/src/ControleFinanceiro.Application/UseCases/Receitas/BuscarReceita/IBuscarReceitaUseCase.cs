using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.BuscarReceita;

public interface IBuscarReceitaUseCase
{
    Task<ReceitaResponseDto> ExecuteAsync(int id);
}