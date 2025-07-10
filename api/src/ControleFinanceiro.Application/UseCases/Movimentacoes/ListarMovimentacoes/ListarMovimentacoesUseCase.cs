using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;

public class ListarMovimentacoesUseCase(IMovimentacoesRepository repository, IUserContext context, IMapper mapper) : IListarMovimentacoesUseCase
{
    public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
        
        var usuarioId = context.UsuarioId;
        
        var movimentacoes = await repository.ListarMovimentacoesPaginadoAsync(usuarioId, request.Pagina, request.Quantidade);
        var total = await repository.ContarMovimentacoesDespesasAsync(usuarioId);
        var movimentacoesResponse = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoes);
        
        return new MovimentacaoPaginadoResponseDto
        {
            Movimentacoes = movimentacoesResponse,
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total
        };
    }
}