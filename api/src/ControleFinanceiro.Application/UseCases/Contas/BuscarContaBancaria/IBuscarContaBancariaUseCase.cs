using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Contas.BuscarContaBancaria;

public interface IBuscarContaBancariaUseCase
{
    Task<ContaBancariaResponseDto> ExecuteAsync(int id);
}