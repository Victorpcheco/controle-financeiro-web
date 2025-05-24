using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class UsuarioService(
        IUsuarioRepository repository,
        IValidator<UsuarioLoginDto> loginValidator,
        IValidator<UsuarioRegistroDto> registroValidator,
        ITokenService tokenService
    ) : IUsuarioService
    {
        private const int RefreshTokenExpirationDays = 1;
        
        public async Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            var resultValidation = await registroValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var usuarioExiste = await repository.ObterUsuarioAsync(dto.Email);
            if (usuarioExiste != null)
                throw new ArgumentException("Usuário já cadastrado no sistema.");
            
            var refreshToken = tokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);
            
            var novoUsuario = new Usuario(dto.NomeCompleto, dto.Email, dto.SenhaHash, refreshToken, expiration);
            await repository.CriarUsuarioAsync(novoUsuario);

            var jwtToken = tokenService.GenerateToken(novoUsuario);

            return new  TokenResponseDto { Token = jwtToken, RefreshToken = refreshToken };
        }

        public async Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto)
        {

            var resultValidation = await loginValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);
            
            var usuario = await repository.ObterUsuarioAsync(dto.Email);

            if (usuario == null)
                throw new ArgumentException("Usuário não encontrado.");

            var token = tokenService.GenerateToken(usuario);
            var refreshToken = tokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

           await repository.AtualizarRefreshTokenAsync(usuario, refreshToken, expiration);
           return new TokenResponseDto { Token = token, RefreshToken = refreshToken };
        }
    }
}
