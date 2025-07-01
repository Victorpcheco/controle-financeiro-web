using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;

public interface IListarMovimentacoesDespesasUseCase 
{
    Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request);
}