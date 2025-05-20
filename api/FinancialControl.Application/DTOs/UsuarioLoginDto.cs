
namespace FinancialControl.Application.DTOs
{
    public class UsuarioLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }
}
