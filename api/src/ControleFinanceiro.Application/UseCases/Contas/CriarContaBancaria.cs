using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Contas;

public class CriarContaBancaria(IContaBancariaRepository repository,
    IValidator<ContaRequest> validator,
    IUserContext userContext) : ICriarContaBancaria
{
    public async Task<bool> CriarConta(ContaRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var usuarioId = userContext.UsuarioId;
        
        var conta = new ContaBancaria(request.NomeConta,request.SaldoInicial, usuarioId);
        
        await repository.CriarContaAsync(conta);
        
        return true;
    }
}