namespace ControleFinanceiro.Application.Dtos;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
}