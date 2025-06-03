using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;

public interface IBuscarDespesaUseCase
{
    Task<DespesaResponseDto> ExecuteAsync(int id);
}