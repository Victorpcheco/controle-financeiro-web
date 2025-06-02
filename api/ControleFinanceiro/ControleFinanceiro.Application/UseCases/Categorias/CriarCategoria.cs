using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class CriarCategoria(
    ICategoriaRepository repository,
    IValidator<CategoriaCriarRequest> validator,
    IUserContext userContext
) : ICriarCategoria
{
    public async Task<bool> CriarNovaCategoria(CategoriaCriarRequest criarRequest)
    {
        var validationResult = await validator.ValidateAsync(criarRequest);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
            
        var categoriaExiste = await repository.BuscarCategoriaPorNomeAsync(criarRequest.NomeCategoria);
        if (categoriaExiste != null)
            throw new InvalidOperationException("Categoria já cadastrada.");

        var usuarioId = userContext.UsuarioId;
            
        var categoria = new Categoria(criarRequest.NomeCategoria, criarRequest.Tipo, usuarioId);
        categoria.AtualizarUsuarioId(usuarioId);
            
        await repository.CriarCategoriaAsync(categoria);
        return true;
    }
}