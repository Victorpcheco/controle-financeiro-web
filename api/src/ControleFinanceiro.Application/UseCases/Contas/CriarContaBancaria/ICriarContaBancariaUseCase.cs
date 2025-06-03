using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Contas.CriarContaBancaria;

public interface ICriarContaBancariaUseCase
{
    Task<bool> ExecuteAsync(ContaBancariaCriarDto contaBancariaCriarDto);
}