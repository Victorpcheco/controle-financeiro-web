using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;

public interface IListarMovimentacoesPorContaBancariaUseCase
{
    Task <MovimentacaoPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request);
}