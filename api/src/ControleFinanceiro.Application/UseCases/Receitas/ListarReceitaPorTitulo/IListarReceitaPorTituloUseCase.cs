using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorTitulo
{
    public interface IListarReceitaPorTituloUseCase
    {
        Task<ReceitaPaginadoResponseDto> ExecuteAsync(string titulo, PaginadoRequestDto request);
    }
}
