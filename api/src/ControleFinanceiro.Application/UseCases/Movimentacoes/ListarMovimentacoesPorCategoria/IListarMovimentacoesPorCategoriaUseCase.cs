using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;

public interface IListarMovimentacoesPorCategoriaUseCase
{
    Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request);
}