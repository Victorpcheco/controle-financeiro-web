using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces;

public interface IRegistrarUsuarioUseCase
{
    Task<TokenResponse> RegistrarUsuarioAsync(RegisterRequest registerRequest);
}