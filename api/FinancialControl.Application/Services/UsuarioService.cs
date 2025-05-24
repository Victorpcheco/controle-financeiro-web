using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<UsuarioRegistroDto> _registerValidator;
        private readonly IValidator<UsuarioLoginDto> _loginValidator;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private const int RefreshTokenExpirationDays = 1;

        public UsuarioService(IUsuarioRepository repository,IValidator<UsuarioLoginDto> loginValidator, IValidator<UsuarioRegistroDto> registerValidtor, IMapper mapper, ITokenService tokenService)
        {
            _usuarioRepository = repository;
            _registerValidator = registerValidtor;
            _loginValidator = loginValidator;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            var resultValidation = await _registerValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var userExisting = await _usuarioRepository.UsuarioExisteAsync(dto.Email);
            if (userExisting != null)
                throw new ArgumentException("Usuário já cadastrado no sistema.");
            
            var refreshToken = _tokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

            // var user = Usuario.Criar(dto.NomeCompleto, dto.Email, dto.SenhaHash, refreshToken, expiration);
            var usuario = new Usuario(dto.NomeCompleto, dto.Email, dto.SenhaHash, refreshToken, expiration);
            await _usuarioRepository.AdicionarUsuarioAsync(usuario);

            var jwtToken = _tokenService.GenerateToken(usuario);

            return new  TokenResponseDto { Token = jwtToken, RefreshToken = refreshToken };
        }

        public async Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto)
        {

            var resultValidation = await _loginValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var user = _mapper.Map<Usuario>(dto);
            user = await _usuarioRepository.UsuarioExisteAsync(user.Email);

            if (user == null)
                throw new ArgumentException("Usuário não encontrado.");

            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

           await _usuarioRepository.AtualizarRefreshTokenAsync(user, refreshToken, expiration);
           return new TokenResponseDto { Token = token, RefreshToken = refreshToken };
        }
        
        public async Task<bool> RemoverUsuarioAsync(string email)
        {
            var usuario = await _usuarioRepository.UsuarioExisteAsync(email);

            await _usuarioRepository.DeletarAsync(usuario);
            return true;
        }

    }
}
