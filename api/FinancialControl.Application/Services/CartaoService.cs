using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services;

public class CartaoService : ICartaoService
{
    private readonly ICartaoRepository _Repository;
    private readonly IValidator<CartaoRequestDto> _Validator;
    private readonly IMapper _mapper;

    public CartaoService(ICartaoRepository Repository, IValidator<CartaoRequestDto> Validator, IMapper mapper)
    {
        _Repository = Repository;
        _Validator = Validator;
        _mapper = mapper;
    }
    
    public async Task<CartaoResponsePaginadoDto> ObterCartoesPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        if (pagina < 1)
        {
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        }
        if (quantidade < 1)
        {
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");
        }
        
        var cartoes = await _Repository.ListarPaginadoAsync(usuarioId, pagina, quantidade);
        
        var cartoesDto = cartoes.Select(x => _mapper.Map<CartaoResponseDto>(x)).ToList();
        var total = await _Repository.ContarAsync(usuarioId);
        
        return new CartaoResponsePaginadoDto
        {
            Total = total,
            Pagina = pagina,
            Quantidade = quantidade,
            Cartoes = cartoesDto
        };
    }
    
    public async Task<Cartao?> ObterCartaoPorIdAsync(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("Id inválido.");
        }
        
        var cartao = await _Repository.BuscarPorIdAsync(id);
        return cartao;
    }
    
    
    public async Task<bool> CriarCartaoAsync(int usuarioId, CartaoRequestDto cartaoRequest)
    {
        var validationResult = await _Validator.ValidateAsync(cartaoRequest);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var cartao = _mapper.Map<Cartao>(cartaoRequest);
        cartao.UsuarioId = usuarioId;

        await _Repository.AdicionarAsync(cartao);
        return true;
    }
    
    public async Task<bool> AtualizarCartaoAsync(int id, CartaoRequestDto cartaoRequest)
    {
        var validationResult = await _Validator.ValidateAsync(cartaoRequest);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var cartaoExiste = await _Repository.BuscarPorIdAsync(id);
        if (cartaoExiste == null)
            throw new ArgumentException("Conta não encontrada.");

        var cartao = _mapper.Map<Cartao>(cartaoRequest);
        cartaoExiste.Nome = cartao.Nome;
        cartaoExiste.DiaFechamentoFatura = cartao.DiaFechamentoFatura;
        cartaoExiste.DiaVencimentoFatura = cartao.DiaVencimentoFatura;
        cartaoExiste.LimiteTotal = cartao.LimiteTotal;
        // cartaoExiste.ContaDePagamentoId = cartao.ContaDePagamento;
        

        await _Repository.AtualizarAsync(cartaoExiste);
        return true;
    }
    
    // public async Task<bool> DeletarCartaoAsync(int usuarioId, int cartaoId)
    // {
    //     var cartao = await _Repository.BuscarPorIdAsync(cartaoId);
    //     if (cartao == null || cartao.UsuarioId != usuarioId)
    //     {
    //         return false;
    //     }
    //
    //     return await _Repository.RemoverAsync(cartao);
    // }
    
}