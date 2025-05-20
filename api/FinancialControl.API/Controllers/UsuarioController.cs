using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("registro")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioRegistroDto dto)
        {
                var token = await _usuarioService.RegistrarUsuarioAsync(dto);
                return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] UsuarioLoginDto dto)
        {
                var token = await _usuarioService.LoginUsuarioAsync(dto);
                return Ok(token);
        }   
    }
}
