using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitas;

public interface IListarReceitasUseCase
{
    Task<ReceitaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request);
}