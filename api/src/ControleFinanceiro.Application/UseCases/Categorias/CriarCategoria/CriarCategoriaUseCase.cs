using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Entities;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias.CriarCategoria;

public class CriarCategoriaUseCase(
    ICategoriaRepository repository,
    IValidator<CategoriaCriarDto> validator,
    IUserContext userContext
) : ICriarCategoriaUseCase
{
    public async Task<bool> ExecuteAsync(CategoriaCriarDto criarDto)
    {
        var validationResult = await validator.ValidateAsync(criarDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        var categoriaExiste = await repository.BuscarCategoriaPorNomeAsync(criarDto.NomeCategoria);
        if (categoriaExiste != null)
            throw new InvalidOperationException("Categoria já cadastrada.");

        var usuarioId = userContext.UsuarioId;
            
        var categoria = new Categoria(criarDto.NomeCategoria, criarDto.Tipo, usuarioId);
        categoria.AtualizarUsuarioId(usuarioId);
            
        await repository.CriarCategoriaAsync(categoria);
        return true;
    }
}