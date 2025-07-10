using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Categorias.CriarCategoria;

public interface ICriarCategoriaUseCase
{
    Task<bool> ExecuteAsync(CategoriaCriarDto criarDto);
}