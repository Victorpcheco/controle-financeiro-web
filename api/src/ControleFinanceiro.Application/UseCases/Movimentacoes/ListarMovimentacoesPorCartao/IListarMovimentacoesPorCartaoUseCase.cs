using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;

public interface IListarMovimentacoesPorCartaoUseCase
{
    Task <MovimentacaoPaginadoResponseDto> ExecuteAsync(int cartaoId, PaginadoRequestDto request);
}