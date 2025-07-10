using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto
{
    public interface IListarMovimentacoesEmAbertoUseCase
    {
        Task<MovimentacoesDashboardPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto);
    }
}
