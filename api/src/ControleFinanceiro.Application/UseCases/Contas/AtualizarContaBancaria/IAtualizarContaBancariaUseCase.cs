using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Contas.AtualizarContaBancaria;

public interface IAtualizarContaBancariaUseCase
{
    Task<bool> ExecuteAsync(int id, ContaBancariaAtualizarDto dto);
}