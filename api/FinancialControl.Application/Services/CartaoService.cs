using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services;

public class CartaoService(ICartaoRepository repository, IValidator<CartaoRequestDto> validator, IMapper mapper) : ICartaoService
{
    
    public async Task<CartaoResponsePaginadoDto> ListarCartaoPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidade < 1)
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");
        
        var cartoes = await repository.ListarCartaoPaginadoAsync(usuarioId, pagina, quantidade);
        
        var cartoesDto = cartoes.Select(x => mapper.Map<CartaoResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync(usuarioId);
        
        return new CartaoResponsePaginadoDto
        {
            Total = total,
            Pagina = pagina,
            Quantidade = quantidade,
            Cartoes = cartoesDto
        };
    }
    
    public async Task<Cartao?> BuscarCartaoPorIdAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");
        
        var cartao = await repository.BuscarCartaoPorId(id);
        if (cartao == null)
            throw new ArgumentException("Cartão não encontrado.");
        
        return cartao;
    }
    
    
    public async Task<bool> CriarCartaoAsync(int usuarioId, CartaoRequestDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var cartao = mapper.Map<Cartao>(dto);
        cartao.AdicionarUsuarioId(usuarioId);
        cartao.AtualizarContaDePagamentoId(dto.ContaDePagamentoId);

        await repository.AdicionarCartaoAsync(cartao);
        return true;
    }
    
    public async Task<bool> AtualizarCartaoAsync(int id, CartaoRequestDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var cartao = await repository.BuscarCartaoPorId(id);
        if (cartao == null)
            throw new ArgumentException("cartão não encontrado.");
        
        cartao.AtualizarCartao(dto.Nome, dto.LimiteTotal, dto.DiaFechamentoFatura, dto.DiaVencimentoFatura, dto.ContaDePagamentoId);
        await repository.AtualizarCartaoAsync(cartao);
        return true;
    }
    
    public async Task<bool> DeletarCartaoAsync(int id)
    {
        var cartao = await repository.BuscarCartaoPorId(id);
        if (cartao == null)
            throw new ArgumentException("Cartão não encontrado.");
    
        await repository.DeletarCartaoAsync(cartao);
        return true;
    }
}