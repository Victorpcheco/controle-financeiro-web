using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Receitas.AtualizarReceita;

public class AtualizarReceitaUseCase(IReceitaRepository repository, IValidator<ReceitaCriarDto> validator) : IAtualizarReceitaUseCase
{
    public async Task<bool> ExecuteAsync(int id, ReceitaCriarDto request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (id <= 0)
            throw new ArgumentException("Id inválido.");

        var receita = await repository.BuscarReceitaPorIdAsync(id);
        if (receita == null)
            throw new KeyNotFoundException("Receita não encontrada.");

        receita.AtualizarReceita(
            request.TituloReceita,
            request.Data,
            request.MesReferenciaId,
            request.CategoriaId,
            request.ContaBancariaId,
            request.Valor,
            request.Realizado
        );

        await repository.AtualizarReceitaAsync(receita);
        return true;
    }
}