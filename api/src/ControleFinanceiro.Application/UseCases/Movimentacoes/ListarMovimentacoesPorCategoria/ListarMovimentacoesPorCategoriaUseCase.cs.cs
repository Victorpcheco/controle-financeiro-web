using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Movimentacoess.ListarMovimentacoesPorCategoria;

public class ListarMovimentacoesPorCategoriaUseCase(
    IMovimentacoesRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarMovimentacoesPorCategoriaUseCase
{
    public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var movimentacoess =
            await repository.ListarMovimentacoesPaginadoPorCategoriaAsync(usuarioId, categoriaId, request.Pagina,
                request.Quantidade);
        var movimentacoessResponse = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoess);
        var total = await repository.ContarMovimentacoesPorCategoriaAsync(usuarioId, categoriaId);

        return new MovimentacaoPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total,
            Movimentacoes = movimentacoessResponse
        };
    }
}