using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class CartaoCriarDtoValidator : AbstractValidator<CartaoCriarDto>
{
    public CartaoCriarDtoValidator()
    {
        RuleFor(x => x.NomeCartao)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("Nome deve ter no máximo 100 caracteres")
            .MinimumLength(2)
            .WithMessage("Nome deve ter pelo menos 2 caracteres");
    }
}