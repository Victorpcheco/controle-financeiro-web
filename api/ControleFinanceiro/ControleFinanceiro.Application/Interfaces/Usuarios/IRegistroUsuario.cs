using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Usuarios;

public interface IRegistroUsuario
{
    Task<TokenResponse> RegistrarUsuario(RegisterRequest registerRequest);
}