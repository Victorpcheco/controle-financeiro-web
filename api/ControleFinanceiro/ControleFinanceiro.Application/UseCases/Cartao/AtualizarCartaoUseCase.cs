using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Cartao;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Cartao;

public class AtualizarCartaoUseCase(ICartaoRepository repository, IValidator<CartaoCriarDto> validator) 
    : IAtualizarCartaoUseCase
{
    public async Task<bool> ExecuteAsync(int id, CartaoCriarDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var cartao = await repository.BuscarCartaoPorId(id);
        if (cartao == null)
            throw new ArgumentException("Cartão não encontrado.");
        
        cartao.AtualizarCartao(dto.NomeCartao);
        await repository.AtualizarCartaoAsync(cartao);
        return true;
    }
}