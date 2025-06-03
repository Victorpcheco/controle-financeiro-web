using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Cartoes.BuscarCartao;

public class BuscarCartaoUseCase(ICartaoRepository repository, IMapper mapper) 
    : IBuscarCartaoUseCase
{
    public async Task<CartaoResponseDto> ExecuteAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");
        
        var buscaCartao = await repository.BuscarCartaoPorId(id);
        if (buscaCartao == null)
            throw new ArgumentException("Cartão não encontrado.");
        
        var cartao = mapper.Map<CartaoResponseDto>(buscaCartao);
        
        return cartao;
    }
}