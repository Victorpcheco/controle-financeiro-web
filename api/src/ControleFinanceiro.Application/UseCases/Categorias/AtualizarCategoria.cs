using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class AtualizarCategoria(
    ICategoriaRepository repository,
    IValidator<CategoriaCriarRequest> validator
) : IAtualizarCategoria
{
    public async Task<bool> AtualizarCategoriaExistente(int id, CategoriaCriarRequest criarRequest)
    {
        var validationResult = await validator.ValidateAsync(criarRequest);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
            
        var categoria = await repository.BuscarCategoriaPorIdAsync(id);
        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
            
        categoria.AtualizarCategoria(criarRequest.NomeCategoria, criarRequest.Tipo);
            
        await repository.AtualizarCategoriaAsync(categoria);
        return true;
    }
}