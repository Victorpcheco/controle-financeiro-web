using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Usuarios;

public interface ILoginUsuario
{
    Task<TokenResponse> Login(LoginRequest request);
}