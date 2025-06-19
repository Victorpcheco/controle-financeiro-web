using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public class ListarDespesaPorDataUseCase(IDespesaRepository repository, IUserContext userContext, IMapper mapper) : IListarDespesaPorDataUseCase
{
    public async Task<DespesaPaginadoResponseDto> ExecuteAsync(string data, PaginadoRequestDto request)
    {
        var usuarioId = userContext.UsuarioId;
        
        var despesas = await repository.ListarDespesasPaginadoPorDataAsync(usuarioId, data, request.Pagina, request.Quantidade);
        var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
        var totalDespesas = await repository.ContarDespesasPorDataAsync(usuarioId, data);

        return new DespesaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalDespesas,
            Despesas = despesasResponse
        };
    }
}