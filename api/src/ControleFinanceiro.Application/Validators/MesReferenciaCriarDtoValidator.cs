using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class MesReferenciaCriarDtoValidator : AbstractValidator<MesReferenciaCriarDto>
{
    public MesReferenciaCriarDtoValidator()
    {
        RuleFor(m => m.NomeMes)
            .NotEmpty()
            .WithMessage("O nome do mês é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O nome do mês deve ter no máximo 50 caracteres.");
    }
}