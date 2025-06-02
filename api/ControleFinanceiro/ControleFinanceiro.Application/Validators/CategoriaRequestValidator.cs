using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class CategoriaRequestValidator : AbstractValidator<CategoriaCriarRequest>
{
    public CategoriaRequestValidator()
    {
        RuleFor(x => x.NomeCategoria)
            .NotEmpty().WithMessage("Nome da categoria é obrigatório.");
        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo de categoria inválido.");
    }
}