using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias.AtualizarCategoria;

public class AtualizarCategoriaUseCase(
    ICategoriaRepository repository,
    IValidator<CategoriaCriarDto> validator
) : IAtualizarCategoriaUseCase
{
    public async Task<bool> ExecuteAsync(int id, CategoriaCriarDto criarDto)
    {
        var validationResult = await validator.ValidateAsync(criarDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
            
        var categoria = await repository.BuscarCategoriaPorIdAsync(id);
        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
            
        categoria.AtualizarCategoria(criarDto.NomeCategoria, criarDto.Tipo);
            
        await repository.AtualizarCategoriaAsync(categoria);
        return true;
    }
}