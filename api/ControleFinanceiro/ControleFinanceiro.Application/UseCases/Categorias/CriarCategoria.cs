using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class CriarCategoria(
    ICategoriaRepository repository,
    IValidator<CategoriaRequest> validator,
    IUserContext userContext
) : ICriarCategoria
{
    public async Task<bool> CriarNovaCategoria(CategoriaRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        var categoriaExiste = await repository.BuscarCategoriaPorNomeAsync(request.NomeCategoria);
        if (categoriaExiste != null)
            throw new InvalidOperationException("Categoria já cadastrada.");

        var usuarioId = userContext.UsuarioId;
            
        var categoria = new Categoria(request.NomeCategoria, request.Tipo, usuarioId);
        categoria.AtualizarUsuarioId(usuarioId);
            
        await repository.CriarCategoriaAsync(categoria);
        return true;
    }
}