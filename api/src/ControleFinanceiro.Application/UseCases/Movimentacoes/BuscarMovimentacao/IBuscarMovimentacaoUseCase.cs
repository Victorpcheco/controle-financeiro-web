using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;

namespace ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;

public interface IBuscarMovimentacaoUseCase
{
    Task<MovimentacaoResponseDto> ExecuteAsync(int id);
}