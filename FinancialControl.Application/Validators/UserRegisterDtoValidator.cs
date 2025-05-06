using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.NomeCompleto)
                .NotEmpty().WithMessage("Nome completo é obrigatório.")
                .Length(1, 100).WithMessage("Nome completo deve ter entre 1 e 100 caracteres.");
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
