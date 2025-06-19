using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorContaBancaria;

public interface IListarReceitaPorContaBancariaUseCase
{
    Task<ReceitaPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request);
}