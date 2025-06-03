using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Usuarios.LoginUsuario;
using ControleFinanceiro.Application.UseCases.Usuarios.RegistroUsuario;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(ILoginUsuarioUseCase login,
    IRegistroUsuarioUseCase register
    ) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<ActionResult> RegistrarUsuario([FromBody] RegisterRequestDto requestDto)
    {
        var token = await register.ExecuteAsync(requestDto);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUsuario([FromBody] LoginRequestDto requestDto)
    {
        var token = await login.ExecuteAsync(requestDto);
        return Ok(token);
    }
}