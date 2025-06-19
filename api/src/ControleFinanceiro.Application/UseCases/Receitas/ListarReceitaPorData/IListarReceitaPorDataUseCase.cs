using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorData;

public interface IListarReceitaPorDataUseCase
{
    Task<ReceitaPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request);
  
}