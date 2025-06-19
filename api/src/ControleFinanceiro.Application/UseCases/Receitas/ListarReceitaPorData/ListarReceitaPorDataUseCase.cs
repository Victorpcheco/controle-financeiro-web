using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorData;

public class ListarReceitaPorDataUseCase(
    IReceitaRepository repository,
    IUserContext userContext,
    IMapper mapper
) : IListarReceitaPorDataUseCase
{
    public async Task<ReceitaPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = userContext.UsuarioId;

        var receitas = await repository.ListarReceitasPaginadoPorDataAsync(usuarioId, data, request.Pagina, request.Quantidade);
        var receitasResponse = mapper.Map<List<ReceitaResponseDto>>(receitas);
        var totalReceitas = await repository.ContarReceitasPorDataAsync(usuarioId, data);

        return new ReceitaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalReceitas,
            Receitas = receitasResponse
        };
    }
}