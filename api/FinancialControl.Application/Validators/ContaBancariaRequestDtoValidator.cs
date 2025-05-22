using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators;

public class ContaBancariaRequestDtoValidator : AbstractValidator<ContaBancariaRequestDto>
{
    public ContaBancariaRequestDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome da conta é obrigatório.");
        RuleFor(x => x.Banco)
            .NotEmpty()
            .WithMessage("O banco é obrigatório.");
        RuleFor(x => x.SaldoInicial)
            .NotEmpty()
            .WithMessage("O saldo incial é obrigatório");
    }       
}