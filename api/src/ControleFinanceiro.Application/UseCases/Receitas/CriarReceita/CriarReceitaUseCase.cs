using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Receitas.CriarReceita;

public class CriarReceitaUseCase(IReceitaRepository repository, IUserContext context, IValidator<ReceitaCriarDto> validator) : ICriarReceitaUseCase
{
    public async Task<bool> ExecuteAsync(ReceitaCriarDto request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var usuarioId = context.UsuarioId;

        var receita = new Receita(
            request.TituloReceita,
            request.Data,
            request.MesReferenciaId,
            request.CategoriaId,
            request.ContaBancariaId,
            request.Valor,
            request.Realizado,
            usuarioId
        );

        await repository.AdicionarReceitaAsync(receita);
        return true;
    }
}