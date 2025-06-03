using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Cartoes.ListarCartao;

public class ListarCartaoPaginadoUseCase(ICartaoRepository repository, IMapper mapper, IUserContext userContext) 
    : IListarCartaoPaginadoUseCase
{
    public async Task<CartaoResponsePaginadoDto> ExecuteAsync(PaginadoRequestDto requestDto)
    {
        if (requestDto.Pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (requestDto.Quantidade < 1)
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");
                
        var usuarioId = userContext.UsuarioId;       
        
        var cartoes = await repository.ListarCartaoPaginadoAsync(usuarioId, requestDto.Pagina, requestDto.Quantidade);
        
        var cartoesDto = cartoes.Select(x => mapper.Map<CartaoResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync(usuarioId);
        
        return new CartaoResponsePaginadoDto
        {
            Total = total,
            Pagina = requestDto.Pagina,
            Quantidade = requestDto.Quantidade,
            Cartoes = cartoesDto
        };
    }
}
