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
        private readonly IValidator<UserRegisterDto> _validator;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private const int RefreshTokenExpirationDays = 1;

        public UserService(IUserRepository repository, IValidator<UserRegisterDto> validator, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _userRepository = repository;
            _validator = validator;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(string token, string refreshToken)> RegisterUserAsync(UserRegisterDto dto)
        {
           var resultValidation = await _validator.ValidateAsync(dto);
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
    }
}
