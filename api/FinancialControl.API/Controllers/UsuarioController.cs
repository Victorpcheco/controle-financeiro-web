using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController(IUsuarioService service) : ControllerBase
    {
        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioRequestDto dto)
        {
                var token = await service.RegistrarUsuarioAsync(dto);
                return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUsuario([FromBody] UsuarioLoginDto dto)
        {
                var token = await service.LoginUsuarioAsync(dto);
                return Ok(token);
        }
    }
}
