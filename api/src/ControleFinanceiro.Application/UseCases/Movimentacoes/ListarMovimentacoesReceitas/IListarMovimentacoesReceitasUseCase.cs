using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarMovimentacoesReceitas
{
    public interface IListarMovimentacoesReceitasUseCase
    {
       Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request);
    }
}
