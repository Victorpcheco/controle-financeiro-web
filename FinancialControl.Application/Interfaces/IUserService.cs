
using FinancialControl.Application.DTOs;

namespace FinancialControl.Application.Interfaces
{
    public interface IUserService
    {
        Task<(string token, string refreshToken)> RegisterUserAsync(UserRegisterDto dto);
    }
}
