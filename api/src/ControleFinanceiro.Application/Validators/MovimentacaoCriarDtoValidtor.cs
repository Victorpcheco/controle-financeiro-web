using ControleFinanceiro.Application.Dtos;
using FluentValidation;

namespace ControleFinanceiro.Application.Validators;

public class MovimentacaoCriarDtoValidtor : AbstractValidator<MovimentacaoCriarDto>
{
    public MovimentacaoCriarDtoValidtor()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título da despesa não pode ser vazio ou nulo.")
            .MaximumLength(100).WithMessage("Título da despesa não pode ter mais de 100 caracteres.");
        RuleFor(x => x.DataVencimento)
            .NotEmpty().WithMessage("A data da despesa não pode ser vazio ou nulo.");
    }
}