using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class AtualizarCategoria(
    ICategoriaRepository repository,
    IValidator<CategoriaRequest> validator
) : IAtualizarCategoria
{
    public async Task<bool> AtualizarCategoriaExistente(int id, CategoriaRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
            
        var categoria = await repository.BuscarCategoriaPorIdAsync(id);
        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
            
        categoria.AtualizarCategoria(request.NomeCategoria, request.Tipo);
            
        await repository.AtualizarCategoriaAsync(categoria);
        return true;
    }
}