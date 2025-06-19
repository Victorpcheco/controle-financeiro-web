using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;

namespace ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;

public interface IBuscarDespesaUseCase
{
    Task<DespesaResponseDto> ExecuteAsync(int id);
}