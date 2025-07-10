using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorTitulo;

public interface IListarMovimentacoesPorTituloUseCase
{
    Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(string titulo, PaginadoRequestDto request);
}