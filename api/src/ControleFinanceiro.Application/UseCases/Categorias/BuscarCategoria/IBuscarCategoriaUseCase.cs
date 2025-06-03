using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Categorias.BuscarCategoria;

public interface IBuscarCategoriaUseCase
{
    Task<CategoriaResponseDto> ExecuteAsync(int id);
}