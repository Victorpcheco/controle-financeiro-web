using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Cartao;

public interface IListarCartaoPaginadoUseCase
{
    Task<CartaoResponsePaginadoDto> ExecuteAsync(ListarCartaoPaginadoRequest request);
}