using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators;

public class CategoriaRequestDtoValidator : AbstractValidator<CategoriaRequestDto>
{
    public CategoriaRequestDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
            .Length(1, 30).WithMessage("Nome da categoria deve ter entre 1 e 30 caracteres.");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo de categoria inválido.");
    }
}