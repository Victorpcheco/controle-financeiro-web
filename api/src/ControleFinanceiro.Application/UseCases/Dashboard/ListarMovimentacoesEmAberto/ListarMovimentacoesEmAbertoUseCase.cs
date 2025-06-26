using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto
{
    public class ListarMovimentacoesEmAbertoUseCase(IDashboardRepository repository, IUserContext userContext, IMapper mapper) : IListarMovimentacoesEmAbertoUseCase
    {
        public async Task<MovimentacoesDashboardPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto)
        {
            if (requestDto.Pagina < 1)
                throw new ArgumentException("A página deve ser maior ou igual a 1.");
            if (requestDto.Quantidade < 1)
                throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");

            var usuarioId = userContext.UsuarioId;

            var movimentacoes = await repository.ListarMovimentacoesEmAbertoAsync(usuarioId, requestDto.Pagina, requestDto.Quantidade);
            if (movimentacoes == null)
                throw new ArgumentException("Nenhuma conta encontrada.");

            var movimentacoesDto = movimentacoes.Select(x => mapper.Map<MovimentacoesResumo>(x)).ToList();
            var total = await repository.ContarTotalAsync(usuarioId);

            return new MovimentacoesDashboardPaginadoResponseDto
            {
                Total = total,
                Pagina = requestDto.Pagina,
                Quantidade = requestDto.Quantidade,
                Movimentacoes = movimentacoesDto,
            };
        }

    }
}
