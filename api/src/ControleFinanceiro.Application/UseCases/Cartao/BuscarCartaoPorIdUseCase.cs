using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Cartao;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Cartao;

public class BuscarCartaoPorIdUseCase(ICartaoRepository repository, IMapper mapper) 
    : IBuscarCartaoPorIdUseCase
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