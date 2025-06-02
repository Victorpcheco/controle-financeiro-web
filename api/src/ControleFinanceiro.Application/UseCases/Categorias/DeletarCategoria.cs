using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class DeletarCategoria(ICategoriaRepository repository) : IDeletarCategoria
{
    public async Task<bool> DeletarCategoriaExistente(int id)
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