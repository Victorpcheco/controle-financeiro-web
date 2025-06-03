using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;

public class ListarDespesasUseCase(IDespesaRepository repository, IUserContext context, IMapper mapper) : IListarDespesasUseCase
{
    public async Task<DespesaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
        
        var usuarioId = context.UsuarioId;
        
        var despesas = await repository.ListarDespesasPaginadoAsync(usuarioId, request.Pagina, request.Quantidade);
        var total = await repository.ContarDespesasAsync(usuarioId);
        var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
        
        return new DespesaPaginadoResponseDto
        {
            Despesas = despesasResponse,
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total
        };
    }
}