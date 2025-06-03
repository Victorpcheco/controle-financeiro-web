using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Contas.AtualizarContaBancaria;

public class AtualizarContaBancariaUseCaseUseCase(IContaBancariaRepository repository,
    IValidator<ContaBancariaAtualizarDto> validator) : IAtualizarContaBancariaUseCase
{
    public async Task<bool> ExecuteAsync(int id, ContaBancariaAtualizarDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await repository.BuscarContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        conta.AtualizarContaBancaria(dto.NomeConta);

        await repository.AtualizarContaAsync(conta);
        return true;
    }
}