using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DespesaController(IListarDespesasUseCase listarDespesasUseCase,
    ICriarDespesaUseCase criarDespesaUseCase,
    IBuscarDespesaUseCase buscarDespesaUseCase,
    IAtualizarDespesaUseCase atualizarDespesaUseCase,
    IDeletarDespesaUseCase despesaUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListarDespesas(      
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        var response = await listarDespesasUseCase.ExecuteAsync(request);
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult> BuscarDespesa(int id)
    {
        var response = await buscarDespesaUseCase.ExecuteAsync(id);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> CriarDespesa([FromBody] DespesaCriarDto request)
    {
        var response = await criarDespesaUseCase.ExecuteAsync(request);
        return Ok(response);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> AtualizarDespesa(int id, [FromBody] DespesaCriarDto request)
    {
        var response = await atualizarDespesaUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletarDespesa(int id)
    {
        await despesaUseCase.ExecuteAsync(id);
        return NoContent();
    }
    
    
}