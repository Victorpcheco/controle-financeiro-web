using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarMovimentacoesReceitas
{
    public class ListarMovimentacoesReceitasUseCase(IMovimentacoesRepository repository, IUserContext context, IMapper mapper) : IListarMovimentacoesReceitasUseCase
    {
        public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request)
        {
            if (request.Pagina <= 0 || request.Quantidade <= 0)
                throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

            var usuarioId = context.UsuarioId;

            var movimentacoes = await repository.ListarMovimentacoesReceitasPaginadoAsync(usuarioId, request.Pagina, request.Quantidade);
            var total = await repository.ContarMovimentacoesReceitasAsync(usuarioId);
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
}
