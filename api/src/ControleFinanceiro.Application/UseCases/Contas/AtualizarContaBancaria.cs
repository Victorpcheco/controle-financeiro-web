using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Contas;

public class AtualizarContaBancaria(IContaBancariaRepository repository,
    IValidator<ContaAtualizarRequest> validator) : IAtualizarContaBancaria
{
    public async Task<bool> AtualizarConta(int id, ContaAtualizarRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await repository.BuscarContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        conta.AtualizarContaBancaria(request.NomeConta);

        await repository.AtualizarContaAsync(conta);
        return true;
    }
}