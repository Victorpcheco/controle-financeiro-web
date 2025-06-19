using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;

public class ListarDespesaPorCategoriaUseCase(
    IDespesaRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarDespesaPorCategoriaUseCase
{
    public async Task<DespesaPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var despesas =
            await repository.ListarDespesasPaginadoPorCategoriaAsync(usuarioId, categoriaId, request.Pagina,
                request.Quantidade);
        var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
        var totalDespesas = await repository.ContarDespesasPorCategoriaAsync(usuarioId, categoriaId);

        return new DespesaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalDespesas,
            Despesas = despesasResponse
        };
    }
}