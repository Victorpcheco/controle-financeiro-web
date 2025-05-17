using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto dto)
        {
                var (jwtToken, refreshToken) = await _userService.RegisterUserAsync(dto);
                return Ok(new UserResponseDto
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDto dto)
        {
                var (jwtToken, refreshToken) = await _userService.LoginUserAsync(dto);
                return Ok(new UserResponseDto
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
        }   
    }
}
