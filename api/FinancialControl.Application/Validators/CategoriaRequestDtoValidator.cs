using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators;

public class CategoriaRequestDtoValidator : AbstractValidator<CategoriaRequestDto>
{
    public CategoriaRequestDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome da categoria é obrigatório.");
        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo de categoria inválido.");
    }
}