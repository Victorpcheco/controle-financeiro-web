using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class ObterCategoria(
    ICategoriaRepository repository,
    IMapper mapper) : IObterCategoria
{
    public async Task<CategoriaResponse> ObterCategoriaPorId(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido");

        var buscaCategoria = await repository.BuscarCategoriaPorIdAsync(id); 
        if (buscaCategoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
            
        var categoria = mapper.Map<CategoriaResponse>(buscaCategoria);

        return categoria;
    }
}