using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Contas.CriarContaBancaria;

public class CriarContaBancariaUseCase(IContaBancariaRepository repository,
    IValidator<ContaBancariaCriarDto> validator,
    IUserContext userContext) : ICriarContaBancariaUseCase
{
    public async Task<bool> ExecuteAsync(ContaBancariaCriarDto contaBancariaCriarDto)
    {
        var validationResult = await validator.ValidateAsync(contaBancariaCriarDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var usuarioId = userContext.UsuarioId;
        
        var conta = new ContaBancaria(contaBancariaCriarDto.NomeConta,contaBancariaCriarDto.SaldoInicial, usuarioId);
        
        await repository.CriarContaAsync(conta);
        
        return true;
    }
}