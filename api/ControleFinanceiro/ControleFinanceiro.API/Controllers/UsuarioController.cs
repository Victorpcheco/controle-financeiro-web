using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Application.Interfaces.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/usuario")]
public class UsuarioController(ILoginUsuario login,
    IRegistroUsuario register
    ) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<ActionResult> RegistrarUsuario([FromBody] RegisterRequest request)
    {
        var token = await register.RegistrarUsuario(request);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUsuario([FromBody] LoginRequest request)
    {
        var token = await login.Login(request);
        return Ok(token);
    }
}