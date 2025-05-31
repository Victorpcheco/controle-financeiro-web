using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/usuario")]
public class UsuarioController(ILoginUsuarioUseCase loginUseCase,
    IRegistrarUsuarioUseCase registerUseCase
    ) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<ActionResult> RegistrarUsuario([FromBody] RegisterRequest request)
    {
        var token = await registerUseCase.RegistrarUsuarioAsync(request);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUsuario([FromBody] LoginRequest request)
    {
        var token = await loginUseCase.LoginUsuarioAsync(request);
        return Ok(token);
    }
}