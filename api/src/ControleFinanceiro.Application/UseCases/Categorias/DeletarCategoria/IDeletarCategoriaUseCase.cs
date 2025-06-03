namespace ControleFinanceiro.Application.UseCases.Categorias.DeletarCategoria;

public interface IDeletarCategoriaUseCase
{
    Task<bool> ExecuteAsync(int id);
}