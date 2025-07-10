using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Cartoes.CriarCartao;

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

        var cartao = new Cartao(dto.NomeCartao, usuarioId);

        await repository.AdicionarCartaoAsync(cartao);
        return true;
    }
}