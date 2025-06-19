using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorCategoria
{
    public interface IListarReceitaPorCategoriaUseCase
    {
        Task<ReceitaPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request);
    }
}
