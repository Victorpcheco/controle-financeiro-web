using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;

public class ListarMovimentacoesPorCartaoUseCase(
    IMovimentacoesRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarMovimentacoesPorCartaoUseCase
{
    public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(int cartaoId, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var movimentacoes =
            await repository.ListarMovimentacoesPaginadoPorCartaoAsync(usuarioId, cartaoId, request.Pagina,
                request.Quantidade);
        var movimentacaoResponse = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoes);
        var total = await repository.ContarMovimentacoesPorCartaoAsync(usuarioId, cartaoId);

        return new MovimentacaoPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total,
            Movimentacoes = movimentacaoResponse
        };
    }
}