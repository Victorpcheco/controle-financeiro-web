using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Cartoes.ListarCartao;

public interface IListarCartaoPaginadoUseCase
{
    Task<CartaoResponsePaginadoDto> ExecuteAsync(PaginadoRequestDto requestDto);
}