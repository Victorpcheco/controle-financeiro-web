using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class ContaBancariaAtualizarDtoValidator : AbstractValidator<ContaBancariaAtualizarDto>
{
    public ContaBancariaAtualizarDtoValidator()
    {
        RuleFor(x => x.NomeConta)
            .NotEmpty().WithMessage("O nome da conta é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da conta deve ter no máximo 100 caracteres.");
    }
}