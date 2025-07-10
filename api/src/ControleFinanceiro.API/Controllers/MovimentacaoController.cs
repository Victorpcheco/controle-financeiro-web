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
using ControleFinanceiro.Application.UseCases.Despesas.ListarMovimentacoesReceitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("api/movimentacoes")]
[Authorize]
public class MovimentacaoController(IListarMovimentacoesUseCase listarDespesasUseCase,
    IListarMovimentacoesReceitasUseCase listarMovimentacoesReceitas,
    ICriarMovimentacaoUseCase criarDespesaUseCase,
    IBuscarMovimentacaoUseCase buscarDespesaUseCase,
    IAtualizarMovimentacaoUseCase atualizarDespesaUseCase,
    IDeletarMovimentacaoUseCase despesaUseCase,
    IListarMovimentacoesPorDataUseCase listarPordataUseCase,
    IListarMovimentacoesPorContaBancariaUseCase listarPorContaUseCase,
    IListarMovimentacoesPorCategoriaUseCase listarDespesaPorCategoriaUseCase,
    IListarMovimentacoesPorTituloUseCase listarPorTituloUseCase,
    IListarMovimentacoesPorCartaoUseCase listarPorCartaoUseCase
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListarMovimentacoes(      
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
    [HttpGet("receitas")]
    public async Task<ActionResult> ListarReceitas(
    [FromQuery] int pagina = 1,
    [FromQuery] int quantidade = 10)
    {
        var request = new PaginadoRequestDto
        {
            Pagina = pagina,
            Quantidade = quantidade
        };
        var response = await listarMovimentacoesReceitas.ExecuteAsync(request);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> BuscarMovimentacao(int id)
    {
        var response = await buscarDespesaUseCase.ExecuteAsync(id);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> CriarMovimentacao([FromBody] MovimentacaoCriarDto request)
    {
        var response = await criarDespesaUseCase.ExecuteAsync(request);
        return Ok(response);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> AtualizarMovimentacao(int id, [FromBody] MovimentacaoCriarDto request)
    {
        var response = await atualizarDespesaUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletarMovimentacao(int id)
    {
        await despesaUseCase.ExecuteAsync(id);
        return NoContent();
    }

    [HttpGet("por-data")]
    public async Task<ActionResult> ListarMovimentacaoPorData([FromQuery] string data, 
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
    public async Task<ActionResult> ListarMovimentacaoPorContaBancaria([FromQuery] int contaId, 
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
    public async Task<ActionResult> ListarMovimentacaoPorCategoria([FromQuery] int categoriaId, 
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
    public async Task<ActionResult> ListarMovimentacaoPorTitulo([FromQuery] string titulo, 
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
    public async Task<ActionResult> ListarMovimentacaoPorTitulo([FromQuery] int cartaoId, 
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