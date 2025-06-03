using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Categorias.ListarCategorias;

public interface IListarCategoriasUseCase
{
    Task<CategoriaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto);
}