
namespace FinancialControl.Application.DTOs
{
    public class UserLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }
}
