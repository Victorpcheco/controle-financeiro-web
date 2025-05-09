using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");
            RuleFor(x => x.SenhaHash)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.")
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
                .WithMessage("Senha deve conter pelo menos uma letra maiúscula, um número e um caractere especial.");
        }
    }
}
