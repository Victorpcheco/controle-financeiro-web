using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators;

public class CartaoRequestDtoValidator : AbstractValidator<CartaoRequestDto>
{
    public CartaoRequestDtoValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O nome do cartão é obrigatório.");
        RuleFor(c => c.LimiteTotal)
            .NotEmpty()
            .WithMessage("O limite do cartão é obrigatório.");
        RuleFor(c => c.DiaFechamentoFatura)
            .NotEmpty()
            .WithMessage("O dia de fechamento do cartão é obrigatório.");
        RuleFor(c => c.DiaVencimentoFatura)
            .NotEmpty()
            .WithMessage("O dia de vencimento do cartão é obrigatório.");
    }
}