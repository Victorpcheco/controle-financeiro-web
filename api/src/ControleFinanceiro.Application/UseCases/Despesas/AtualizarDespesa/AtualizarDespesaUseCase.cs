using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;

public class AtualizarDespesaUseCase(IDespesaRepository repository, IValidator<DespesaCriarDto> validator) 
    : IAtualizarDespesaUseCase
{
    public async Task<bool> ExecuteAsync(int id, DespesaCriarDto request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id <= 0)
            throw new ArgumentException("Id inválido.");
   
        var despesa = await repository.BuscarDespesaPorIdAsync(id);
        if (despesa == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        despesa.AtualizarDespesa(request.TituloDespesa, request.Data, request.MesReferenciaId, request.CategoriaId,
            request.CartaoId, request.ContaBancariaId, request.Valor, request.Realizado);
        
        await repository.AtualizarDespesaAsync(despesa);
        return true;
    }
}