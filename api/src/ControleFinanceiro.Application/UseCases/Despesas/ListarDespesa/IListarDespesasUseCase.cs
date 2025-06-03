using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;

public interface IListarDespesasUseCase 
{
    Task<DespesaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request);
}