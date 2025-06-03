using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;

public class CriarDespesaUseCase(IDespesaRepository repository, IUserContext context, IValidator<DespesaCriarDto> validator) : ICriarDespesaUseCase
{
    public async Task<bool> ExecuteAsync(DespesaCriarDto request)
    {
        var validationResult = validator.ValidateAsync(request);
        if (!validationResult.Result.IsValid)
            throw new ValidationException(validationResult.Result.Errors);
        
        var usuarioId = context.UsuarioId;
        
        var despesa = new Despesa(request.TituloDespesa, request.Data, request.MesReferenciaId, request.CategoriaId,
            request.CartaoId, request.ContaBancariaId, request.Valor, request.Realizado, usuarioId);
        
        await repository.AdicionarDespesaAsync(despesa);
        return true;
    }
}
