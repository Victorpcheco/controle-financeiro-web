using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorTitulo;
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
    IDeletarDespesaUseCase despesaUseCase,
    IListarDespesaPorDataUseCase listarPordataUseCase,
    IListarDespesaPorContaBancariaUseCase listarPorContaUseCase,
    IListarDespesaPorCategoriaUseCase listarDespesaPorCategoriaUseCase,
    IListarDespesaPorTituloUseCase listarPorTituloUseCase,
    IListarDespesaPorCartaoUseCase listarPorCartaoUseCase
    ) : ControllerBase
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

    [HttpGet("por-data")]
    public async Task<ActionResult> ListarDespesaPorData([FromQuery] string data, 
        [FromQuery] int pagina = 1, 
        [FromQuery] int quantidade = 10)
    {
        
        if (!DateOnly.TryParse(data, out var dataConvertida))
            return BadRequest("Data inválida. Use o formato 'yyyy-MM-dd'.");
        
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        
        var response = await listarPordataUseCase.ExecuteAsync(dataConvertida, request);
        return Ok(response);
    }

    [HttpGet("por-conta")]
    public async Task<ActionResult> ListarDespesaPorContaBancaria([FromQuery] int contaId, 
        [FromQuery] int pagina = 1, 
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        
        var response = await listarPorContaUseCase.ExecuteAsync(contaId, request);
        return Ok(response);
    }

    [HttpGet("por-categoria")]
    public async Task<ActionResult> ListarDespesaPorCategoria([FromQuery] int categoriaId, 
        [FromQuery] int pagina = 1, 
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        
        var response = await listarDespesaPorCategoriaUseCase.ExecuteAsync(categoriaId, request);
        return Ok(response);
    }

    [HttpGet("por-titulo")]
    public async Task<ActionResult> ListarDespesaPorTitulo([FromQuery] string titulo, 
        [FromQuery] int pagina = 1, 
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        
        var response = await listarPorTituloUseCase.ExecuteAsync(titulo, request);
        return Ok(response);
    }
    
    [HttpGet("por-cartao")]
    public async Task<ActionResult> ListarDespesaPorTitulo([FromQuery] int cartaoId, 
        [FromQuery] int pagina = 1, 
        [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        
        var response = await listarPorCartaoUseCase.ExecuteAsync(cartaoId, request);
        return Ok(response);
    }
}