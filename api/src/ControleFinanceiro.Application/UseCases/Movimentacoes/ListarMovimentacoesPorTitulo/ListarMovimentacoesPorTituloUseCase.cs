using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorTitulo;

public class ListarMovimentacoesPorTituloUseCase(
    IMovimentacoesRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarMovimentacoesPorTituloUseCase
{
    public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(string titulo, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var movimentacoes =
            await repository.ListarMovimentacoesPaginadoPorTituloAsync(usuarioId, titulo, request.Pagina,
                request.Quantidade);
        var movimentacaoResponseDtos = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoes);
        var total = await repository.ContarMovimentacoesPorTituloAsync(usuarioId, titulo);

        return new MovimentacaoPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total,
            Movimentacoes = movimentacaoResponseDtos
        };
    }
}