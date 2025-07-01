using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public interface IListarMovimentacoesPorDataUseCase
{
    Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request);
}