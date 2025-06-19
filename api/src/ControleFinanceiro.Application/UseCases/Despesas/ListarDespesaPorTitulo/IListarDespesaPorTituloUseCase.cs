using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorTitulo;

public interface IListarDespesaPorTituloUseCase
{
    Task<DespesaPaginadoResponseDto> ExecuteAsync(string titulo, PaginadoRequestDto request);
}