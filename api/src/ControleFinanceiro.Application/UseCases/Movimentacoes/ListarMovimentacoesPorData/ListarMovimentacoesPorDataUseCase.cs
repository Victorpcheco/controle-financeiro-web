using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public class ListarMovimentacoesPorDataUseCase(IMovimentacoesRepository repository, IUserContext userContext, IMapper mapper) : IListarMovimentacoesPorDataUseCase
{
    public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
        
        var usuarioId = userContext.UsuarioId;
        
        var movimentacoes = await repository.ListarMovimentacoesPaginadoPorDataAsync(usuarioId, data, request.Pagina, request.Quantidade);
        var movimentacaoResponseDtos = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoes);
        var total = await repository.ContarMovimentacoesPorDataAsync(usuarioId, data);

        return new MovimentacaoPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total,
            Movimentacoes = movimentacaoResponseDtos
        };
    }
    
    
}