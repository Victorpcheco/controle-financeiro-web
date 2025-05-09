using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserRegisterDto> _registerValidator;
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private const int RefreshTokenExpirationDays = 1;

        public UserService(IUserRepository repository,IValidator<UserLoginDto> loginValidator, IValidator<UserRegisterDto> registerValidtor, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _userRepository = repository;
            _registerValidator = registerValidtor;
            _loginValidator = loginValidator;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(string token, string refreshToken)> RegisterUserAsync(UserRegisterDto dto)
        {
           var resultValidation = await _registerValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var userMapper = _mapper.Map<User>(dto);

            var jwtToken = _jwtTokenService.GenerateToken(userMapper);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

            var userExisting = await _userRepository.UserExistsAsync(dto.Email);
            if (userExisting != null)
                throw new ArgumentException("Usuário já cadastrado no sistema.");

            var user = User.Criar(dto.NomeCompleto, dto.Email, dto.SenhaHash, refreshToken, expiration);
            await _userRepository.AddUserAsync(user);

           return (jwtToken, refreshToken);
        }

        public async Task<(string token, string refreshToken)> LoginUserAsync(UserLoginDto dto)
        {

            var resultValidation = await _loginValidator.ValidateAsync(dto);
            if (!resultValidation.IsValid)
                throw new ValidationException(resultValidation.Errors);

            var user = _mapper.Map<User>(dto);
            user = await _userRepository.UserExistsAsync(user.Email);

            if (user == null)
                throw new ArgumentException("Usuário não encontrado.");

            var token = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

           await _userRepository.UpdateRefreshTokenAsync(user, refreshToken, expiration);
           return (token, refreshToken);
        }

    }
}
