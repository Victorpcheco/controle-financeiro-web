using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;

public interface IListarDespesaPorCategoriaUseCase
{
    Task<DespesaPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request);
}