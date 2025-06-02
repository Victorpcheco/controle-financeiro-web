using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Cartao;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Cartao;

public class ListarCartaoPaginadoUseCase(ICartaoRepository repository, IMapper mapper, IUserContext userContext) 
    : IListarCartaoPaginadoUseCase
{
    public async Task<CartaoResponsePaginadoDto> ExecuteAsync(ListarCartaoPaginadoRequest request)
    {
        if (request.Pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (request.Quantidade < 1)
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");
                
        var usuarioId = userContext.UsuarioId;       
        
        var cartoes = await repository.ListarCartaoPaginadoAsync(usuarioId, request.Pagina, request.Quantidade);
        
        var cartoesDto = cartoes.Select(x => mapper.Map<CartaoResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync(usuarioId);
        
        return new CartaoResponsePaginadoDto
        {
            Total = total,
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Cartoes = cartoesDto
        };
    }
}
