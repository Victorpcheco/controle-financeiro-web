using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces;

public interface ILoginUsuarioUseCase
{
    Task<TokenResponse> LoginUsuarioAsync(LoginRequest request);
}