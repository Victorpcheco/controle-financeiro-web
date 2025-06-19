using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;

public interface IListarDespesaPorCartaoUseCase
{
    Task <DespesaPaginadoResponseDto> ExecuteAsync(int cartaoId, PaginadoRequestDto request);
}