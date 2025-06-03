using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias.DeletarCategoria;

public class DeletarCategoriaUseCase(ICategoriaRepository repository) : IDeletarCategoriaUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido");

        var categoria = await repository.BuscarCategoriaPorIdAsync(id);
        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        await repository.DeletarCategoriaAsync(categoria);
        return true;
    }
}