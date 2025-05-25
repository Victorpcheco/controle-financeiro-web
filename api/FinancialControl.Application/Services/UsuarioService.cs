﻿using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class UsuarioService(
        IUsuarioRepository repository,
        IValidator<UsuarioLoginDto> loginValidator,
        IValidator<UsuarioRequestDto> registroValidator,
        ITokenService tokenService
    ) : IUsuarioService
    {
        private const int RefreshTokenExpirationDays = 1;
        
        public async Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRequestDto dto)
        {
            var resultValidation = await registroValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var usuarioExiste = await repository.BuscarUsuarioPorEmailAsync(dto.Email);
            if (usuarioExiste != null)
                throw new ArgumentException("Usuário já cadastrado no sistema.");
            
            var refreshToken = tokenService.GerarRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);
            
            var novoUsuario = new Usuario(dto.NomeCompleto, dto.Email, dto.SenhaHash, refreshToken, expiration);
            await repository.CriarUsuarioAsync(novoUsuario);

            var jwtToken = tokenService.GerarToken(novoUsuario);

            return new  TokenResponseDto { Token = jwtToken, RefreshToken = refreshToken };
        }

        public async Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto)
        {

            var resultValidation = await loginValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);
            
            var usuario = await repository.BuscarUsuarioPorEmailAsync(dto.Email);

            if (usuario == null)
                throw new ArgumentException("Usuário não encontrado.");

            var token = tokenService.GerarToken(usuario);
            var refreshToken = tokenService.GerarRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

           await repository.AtualizarRefreshTokenAsync(usuario, refreshToken, expiration);
           return new TokenResponseDto { Token = token, RefreshToken = refreshToken };
        }
    }
}
