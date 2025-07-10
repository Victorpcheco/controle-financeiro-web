using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;

public interface IListarMovimentacoesUseCase 
{
    Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request);
}