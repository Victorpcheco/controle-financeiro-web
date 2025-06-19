using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public interface IListarDespesaPorDataUseCase
{
    Task<DespesaPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request);
}