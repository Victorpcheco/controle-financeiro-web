using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Categorias.AtualizarCategoria;

public interface IAtualizarCategoriaUseCase
{
    Task<bool> ExecuteAsync(int id, CategoriaCriarDto criarDto);
}