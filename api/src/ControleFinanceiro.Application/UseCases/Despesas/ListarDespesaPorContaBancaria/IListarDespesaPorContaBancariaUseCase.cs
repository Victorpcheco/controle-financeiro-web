using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;

public interface IListarDespesaPorContaBancariaUseCase
{
    Task <DespesaPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request);
}