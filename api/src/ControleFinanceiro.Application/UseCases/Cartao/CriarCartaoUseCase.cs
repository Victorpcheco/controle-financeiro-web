using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Cartao;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Cartao;

public class CriarCartaoUseCase(ICartaoRepository repository, IValidator<CartaoCriarDto> validator, IUserContext userContext) 
    : ICriarCartaoUseCase
{
    public async Task<bool> ExecuteAsync(CartaoCriarDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var usuarioId = userContext.UsuarioId;

        var cartao = new CartaoCredito(dto.NomeCartao, usuarioId);

        await repository.AdicionarCartaoAsync(cartao);
        return true;
    }
}