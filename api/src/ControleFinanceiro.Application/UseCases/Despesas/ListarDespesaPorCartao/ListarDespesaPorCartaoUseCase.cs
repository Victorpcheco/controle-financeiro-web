using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;

public class ListarDespesaPorCartaoUseCase(
    IDespesaRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarDespesaPorCartaoUseCase
{
    public async Task<DespesaPaginadoResponseDto> ExecuteAsync(int cartaoId, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var despesas =
            await repository.ListarDespesasPaginadoPorCartaoAsync(usuarioId, cartaoId, request.Pagina,
                request.Quantidade);
        var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
        var totalDespesas = await repository.ContarDespesasPorCartaoAsync(usuarioId, cartaoId);

        return new DespesaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalDespesas,
            Despesas = despesasResponse
        };
    }
}