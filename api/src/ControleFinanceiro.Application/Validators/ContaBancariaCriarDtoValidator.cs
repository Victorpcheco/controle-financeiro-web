using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class ContaBancariaCriarDtoValidator : AbstractValidator<ContaBancariaCriarDto>
{
    public ContaBancariaCriarDtoValidator()
    {
        RuleFor(x => x.NomeConta)
            .NotEmpty()
            .WithMessage("O nome da conta é obrigatório.")
            .Length(1, 100)
            .WithMessage("O nome da conta deve ter entre 1 e 100 caracteres.");
        RuleFor(x => x.SaldoInicial)
            .NotEmpty()
            .WithMessage("O saldo incial da conta é obrigatório");
    }
}